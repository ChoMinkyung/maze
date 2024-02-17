using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Range")]
    [SerializeField]
    float _detectRange = 10f;
    [SerializeField]
    float _meleeAttackRange = 5f;
    [SerializeField]
    float _patrolRange = 5f;

    [Header("Movement")]
    [SerializeField]
    float _movementSpeed = 1f;
    [SerializeField]
    float _rotationSpeed = 5f;

    Vector3 _originPos = default;
    BehaviorTreeRunner _BTRunner = null;
    Transform _detectedPlayer = null;
    Animator _animator = null;

    const string _ATTACK_ANIM_STATE_NAME = "Attack";
    const string _ATTACK_ANIM_TRIGGER_NAME = "attack";

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _BTRunner = new BehaviorTreeRunner(SettingBT());

        _originPos = transform.position;
    }

    private void Update()
    {
        _BTRunner.Operate();
    }

    INode SettingBT()
    {
        return new SelectorNode
        (
            new List<INode>()
            {
                new SequenceNode
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckMeleeAttacking),
                        new ActionNode(CheckEnemyWithinMeleeAttackRange),
                        new ActionNode(DoMeleeAttack),
                    }
                ),
                new SequenceNode
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckDetectEnemy),
                        new ActionNode(MoveToDetectEnemy),
                    }
                ),
                new SequenceNode
                (
                    new List<INode>()
                    {
                        new ActionNode(CheckMyPos),
                        new ActionNode(MoveToOriginPosition),
                    }
                ),
                new ActionNode(DoPatrol),
            }
        );

    }

    bool IsAnimationRunning(string stateName)
    {
        if(_animator != null)
        { 
            if(_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                var normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                return normalizedTime != 0 && normalizedTime < 1f;
            }
        }

        return false;
    }

    #region Attack Node
    INode.ENodeState CheckMeleeAttacking()
    {
        if (IsAnimationRunning(_ATTACK_ANIM_STATE_NAME))
        {

            return INode.ENodeState.ENS_Running;
        }

        return INode.ENodeState.ENS_Success;
    }

    INode.ENodeState CheckEnemyWithinMeleeAttackRange()
    {
        if (_detectedPlayer != null)
        {
            if(Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
            {

                return INode.ENodeState.ENS_Success;
            }
        }
        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState DoMeleeAttack()
    {

        if (_detectedPlayer != null)
        {

            //_animator.SetTrigger(_ATTACK_ANIM_TRIGGER_NAME);
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region Detect & Move Node
    INode.ENodeState CheckDetectEnemy()
    {

        var overlapColliders = Physics.OverlapSphere(transform.position, _detectRange, LayerMask.GetMask("Player"));

        if(overlapColliders != null && overlapColliders.Length > 0)
        {

            _detectedPlayer = overlapColliders[0].transform;

            return INode.ENodeState.ENS_Success;
        }

        _detectedPlayer = null;

        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState MoveToDetectEnemy()
    {

        if (_detectedPlayer != null)
        {
            Vector3 targetDirection = (_detectedPlayer.position - transform.position).normalized;

            // 회전
            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }

            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
            {

                return INode.ENodeState.ENS_Success;
            }

            transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);



            return INode.ENodeState.ENS_Running;
        }
        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region Move Origin Pos Node
    INode.ENodeState CheckMyPos()
    {
        Vector3 targetDistance = _originPos - transform.position;
        targetDistance.y = 0;

        float patrolRangeSqr = _patrolRange * _patrolRange;
        if(Vector3.SqrMagnitude(targetDistance) < patrolRangeSqr)
        {
            return INode.ENodeState.ENS_Failure;

        }

        return INode.ENodeState.ENS_Success;
    }

    INode.ENodeState MoveToOriginPosition()
    {
        Vector3 targetDistance = _originPos - transform.position;
        targetDistance.y = 0;

        if (Vector3.SqrMagnitude(targetDistance) <  float.Epsilon * float.Epsilon)
        {
            return INode.ENodeState.ENS_Success;
        }
        else
        {
            Vector3 targetDirection = targetDistance.normalized;
            // 회전
            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
            transform.position = Vector3.MoveTowards(transform.position, _originPos, Time.deltaTime * _movementSpeed);

            return INode.ENodeState.ENS_Running;
        }
    }
    #endregion

    #region Do Patrol
    INode.ENodeState DoPatrol()
    {
        if(CheckDetectEnemy()==INode.ENodeState.ENS_Success)
        {
            return INode.ENodeState.ENS_Failure;

        }

        MoveForward();
        if (ShouldRotate())
        {
            RotateRandom();
        }
        return INode.ENodeState.ENS_Running;


    }

    bool ShouldRotate()
    {
        // 이동 중인 방향으로 Ray를 쏴서 벽과 충돌 여부를 확인
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f, LayerMask.GetMask("Wall")))
        {
            return true;
        }

        // 행동반경을 벗어났는지 확인
        Vector3 targetDistance = _originPos - transform.position;
        targetDistance.y = 0;

        float patrolRangeSqr = _patrolRange * _patrolRange;
        if (Vector3.SqrMagnitude(targetDistance) > patrolRangeSqr)
        {
            return true;
        }

        return false;
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * _movementSpeed * Time.deltaTime);
    }

    void RotateRandom()
    {
        float randomAngle = Random.Range(0, 3) * 90f; // 0, 1, 2 중에서 랜덤으로 선택한 후 90을 곱하여 90도, 180도, -90도 중에서 선택
        transform.Rotate(Vector3.up, randomAngle);
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            RotateRandom();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _meleeAttackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_originPos, _patrolRange);
    }
}
