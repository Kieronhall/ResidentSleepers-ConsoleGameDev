using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : State
{
    Agent owner;

    public WanderState(Agent owner, StateManager sm) : base(owner, sm)
    {

    }

    public override void Enter()
    {
        //Debug.Log("Entering Wander");
        agent.ResumeLastGoal();
        agent.animWalkingOn();
        agent.animShootingOff();
        agent.animRunningOff();
    }

    public override void Execute()
    {
        //Debug.Log("Executing Wander");
        agent.WanderFunc();
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Wander");
        agent.animWalkingOff();
    }

}
