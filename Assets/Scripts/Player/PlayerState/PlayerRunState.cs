using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerController player { get; set; }
    public PlayerStateMachine stateMachine { get; set; }

    public PlayerRunState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
        player = stateMachine.playerController;
    }

    public void Execute()
    {
        player.SetSpeed(3);
        player.MoveInput();
        
    }

    public void OnStateEnter()
    {
        player.animator.SetBool("isRun", true);
        player.animator.SetBool("isWalk", true);

    }

    public void OnStateExit()
    {
        player.animator.SetBool("isRun", false);
        player.animator.SetBool("isWalk", false);

    }
}
