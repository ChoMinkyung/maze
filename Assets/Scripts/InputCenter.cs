using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCenter : MonoBehaviour
{
    [SerializeField] public PlayerController playerController;
    [SerializeField] public InputHandler inputHandler;

    private void Start()
    {
        inputHandler.OnPlayerWalkInput += ChangeWalkState;
        inputHandler.OnPlayerRunInput += ChangeRunState;
        inputHandler.OnPlayerIdle += ChangeIdleState;
        inputHandler.OnPlayerJumpInput += ChangeJumpState;
    }


    void ChangeWalkState()
    {
        playerController.playerStateMachine.ChangeState(PlayerEnum.Walk);
    }

    void ChangeRunState()
    {
        playerController.playerStateMachine.ChangeState(PlayerEnum.Run);

    }

    void ChangeIdleState()
    {
        playerController.playerStateMachine.ChangeState(PlayerEnum.Idle);
    }

    void ChangeJumpState()
    {
        playerController.playerStateMachine.ChangeState(PlayerEnum.Jump);
    }
}
