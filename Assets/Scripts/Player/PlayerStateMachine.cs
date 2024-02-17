using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerEnum { Idle, Walk, Run, Jump, Loose}

public class PlayerStateMachine : MonoBehaviour
{
    [Header("플레이어 컨트롤러")]
    [SerializeField] public PlayerController playerController;
    [HideInInspector] public PlayerState curState;

    public Dictionary<PlayerEnum, PlayerState> playerStateDictionary;

    private void Awake()
    {
        playerStateDictionary = new Dictionary<PlayerEnum, PlayerState>
        {
            {PlayerEnum.Idle, new PlayerIdleState(this) },
            {PlayerEnum.Walk, new PlayerWalkState(this) },
            {PlayerEnum.Run, new PlayerRunState(this) },
            {PlayerEnum.Jump, new PlayerJumpState(this) },
            {PlayerEnum.Loose, new PlayerLooseState(this) }
        };

        if(playerStateDictionary.TryGetValue(PlayerEnum.Idle, out PlayerState newState))
        {
            curState = newState;
            curState.OnStateEnter();
        }
    }

    public void ChangeState(PlayerEnum newStateType)
    {
        if(curState==null)
        {
            return;
        }

        curState.OnStateExit();

        if(playerStateDictionary.TryGetValue(newStateType, out PlayerState newState))
        {
            newState.OnStateEnter();
            curState = newState;
        }
    }


}
