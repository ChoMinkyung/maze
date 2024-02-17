using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{

    [Header("플레이어 상태 기계")]
    [SerializeField] public PlayerStateMachine playerStateMachine;

    [SerializeField] public Animator animator;
    public Vector3 moveDirection;
    public float speed = 1f;
    public float rotationSpeed = 0.5f; // 회전 속도

    private Vector3 originPos;

    int hp = 3;

    public delegate void PlayerHandle();
    public event PlayerHandle OnPlayerMonsterCollision;
    public event PlayerHandle OnPlayerChestCollision;

    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    bool isChestAnim = false;

    public bool ChestAnim
    {
        get { return isChestAnim;  }
        set { isChestAnim = value; }
    }

    void Start()
    {
        originPos = transform.position;

    }

    void FixedUpdate()
    {
        if (!isChestAnim)
        {
            if (playerStateMachine.curState != null)
            {
                playerStateMachine.curState.Execute();
            }
        }
    }

    public void MoveInput()
    {
        float horizontalInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        float verticalInput = UnityEngine.Input.GetAxisRaw("Vertical");

        // 이동
        Vector3 moveDirection = new Vector3(0, 0, verticalInput).normalized;
        Vector3 movement = moveDirection * speed * Time.deltaTime;
        transform.Translate(movement);

        // 회전
        if (horizontalInput != 0)
        {
            float rotationAmount = horizontalInput * rotationSpeed;
            transform.Rotate(Vector3.up, rotationAmount);
        }

    }

    public void ResetPosition()
    {
        transform.position = originPos;
    }

    public void SetSpeed(float Speed)
    {
        speed = Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnPlayerMonsterCollision?.Invoke();
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Chest"))
        {
            isChestAnim = true;
            OnPlayerChestCollision?.Invoke();
        }
    }
}
