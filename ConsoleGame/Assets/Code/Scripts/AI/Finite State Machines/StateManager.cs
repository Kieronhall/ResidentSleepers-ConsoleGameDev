using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    private Stack stack;

    public void Init(State startState)
    {
        this.stack = new Stack();
        stack.Push(startState);
        startState.Enter();
    }
    
    public bool popState()
    {
        if (stack.Count > 0)
        {
            getCurrState().Exit();
            stack.Pop();
            return true;
        }
        else return false;
    }

    public bool pushState(State _pushme)
    {
        if (stack.Peek() != _pushme)
        {
            stack.Push(_pushme);
            getCurrState().Enter();
            return true;
        }
        else return false;
    }

    public State getCurrState()
    {
        return stack.Count > 0 ? (State)stack.Peek() : null;
    }

    public void Update()
    {
        if (getCurrState() != null) getCurrState().Execute();
    }

    public string GetCurrentStateName()
    {
        State currentState = getCurrState();
        return currentState != null ? currentState.GetType().Name : "No State";
    }

}
