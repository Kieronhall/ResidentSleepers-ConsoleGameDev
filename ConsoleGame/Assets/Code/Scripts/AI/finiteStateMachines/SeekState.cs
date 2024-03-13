using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
    Agent owner;

    public SeekState(Agent owner, StateManager sm) : base(owner, sm)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering Seek");

    }

    public override void Execute()
    {
        Debug.Log("Executing Seek");
        agent.agentAnimRun();
        agent.agentGoToPlayer();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Seek");
    }
}



