using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : State
{
    Agent agent;
    private float stopDistance = 3f; // The distance at which the agent stops and transitions to shooting.

    public SeekState(Agent agent, StateManager sm) : base(agent, sm)
    {
        this.agent = agent;
    }

    public override void Enter()
    {
        //Debug.Log("Entering Seek");
    }

    public override void Execute()
    {
        //Debug.Log("Executing Seek");

        agent.agentAnimRun();
        agent.agentGoToPlayer();


        float distanceToPlayer = agent.DistanceToPlayer();
        if (distanceToPlayer <= stopDistance)
        {
            // WHEN AGENT IS CLOSE ENOUGH
            //sm.pushState(shooting);
            sm.pushState(new ShootingState(agent, sm));
            Debug.Log("CLOSE ENOUGH TO SHOOT");
        }
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Seek");
    }
}



