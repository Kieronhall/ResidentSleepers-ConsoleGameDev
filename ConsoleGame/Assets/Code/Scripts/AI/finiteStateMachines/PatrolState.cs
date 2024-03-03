using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    Agent owner;

    public PatrolState(Agent owner, StateManager sm) : base(owner, sm)
    {

    }

    public override void Enter()
    {
        Debug.Log("Entering Patrol");
        agent.ResumeLastGoal();
    }

    public override void Execute()
    {
        Debug.Log("Executing Patrol");
        agent.agentAnimWalk();
        agent.PatrolFunc();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Patrol");
    }
}


