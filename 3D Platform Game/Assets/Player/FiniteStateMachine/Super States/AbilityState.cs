using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityState : State
{
    protected bool isAbilityDone;
    private bool isGrounded;

    public AbilityState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        isGrounded = player.isGrounded;
        if (isAbilityDone)
        {
            if(isGrounded && player.currentVelocity.y <= 0)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else 
            {
                stateMachine.ChangeState(player.inAirState);
            }
        }
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
    }
}
