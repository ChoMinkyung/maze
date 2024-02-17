using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooseState : PlayerState
{
    public PlayerController player { get; set; }
    public PlayerStateMachine stateMachine { get; set; }

    public PlayerLooseState(PlayerStateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
        player = stateMachine.playerController;
    }

    public void Execute()
    {

    }

    public void OnStateEnter()
    {

    }

    public void OnStateExit()
    {

    }
}
