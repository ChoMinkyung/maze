using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerState
{
    PlayerController player { get; set; }
    PlayerStateMachine stateMachine { get; set; }

    void Execute();
    void OnStateEnter();
    void OnStateExit();
}
