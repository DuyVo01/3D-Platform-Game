using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    public Player player;
    public StateMachine stateMachine;

    public State(Player player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {

    }

    public virtual void LogicalUpdate()
    {

    }

    public virtual void PhysicalUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
