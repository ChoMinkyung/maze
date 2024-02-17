using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerWalkState : PlayerState
{
    public PlayerController player { get; set; }
    public PlayerStateMachine stateMachine { get; set; }

    public PlayerWalkState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
        player = stateMachine.playerController;
    }

    public void Execute()
    {
        player.animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
        player.animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));


        player.SetSpeed(1);
        player.MoveInput();

    }

    public void OnStateEnter()
    {
        player.animator.SetBool("isWalk", true);

    }

    public void OnStateExit()
    {
        player.animator.SetBool("isWalk", false);

    }
}
