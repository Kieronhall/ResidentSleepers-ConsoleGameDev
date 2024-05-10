using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundState : State
{
    Agent owner;

    public LookAroundState(Agent owner, StateManager sm): base(owner, sm)
    {

    }

    public override void Enter()
    {
        //Debug.Log("Entering LookAround");
    }

    public override void Execute()
    {
        //Debug.Log("Executing LookAround");


    }

    public override void Exit()
    {
        //Debug.Log("Exiting LookAround");
    }
}
