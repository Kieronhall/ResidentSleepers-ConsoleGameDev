using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingState : State
{
    Agent owner;

    public ShootingState(Agent owner, StateManager sm): base(owner, sm)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering Shooting");
        agent.agentAnimIdle();
    }

    public override void Execute()
    {
        Debug.Log("Executing Shooting");
        agent.StandStill();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Shooting");
    }
}
