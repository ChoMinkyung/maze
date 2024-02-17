using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerController player { get; set; }
    public PlayerStateMachine stateMachine { get; set; }

    public PlayerJumpState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
        player = stateMachine.playerController;
    }

    public void Execute()
    {
        //player.Jump();
    }

    public void OnStateEnter()
    {
        player.animator.SetBool("isJump", true);
    }

    public void OnStateExit()
    {
        player.animator.SetBool("isJump", false);

    }
}
