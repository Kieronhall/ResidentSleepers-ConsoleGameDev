using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        agent.animShootingOn();
        agent.animRunningOff();
        agent.animWalkingOff();
        agent.GunReposition(new Vector3 (-0.0102f, 0.0235f, -0.0356f), Quaternion.Euler(-3.768f, -150.63f, -181.568f), "gun");
    }

    public override void Execute()
    {
        Debug.Log("Executing Shooting");
        agent.StandStill();
        agent.PopShooting();
        //agent.agentAnimShoot();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Shooting");
        agent.ShootingNotActive();
        agent.animShootingOff();
        agent.animRunningOn();
        agent.GunReposition(new Vector3(-0.0179f, 0.0269f, -0.0171f), Quaternion.Euler(-29.43f, -91.69f, -182f), "gun");
    }
}
