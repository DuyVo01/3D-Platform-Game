using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : GroundState
{
    public IdleState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.playerRB.velocity = Vector3.zero;
        Debug.Log("Enter IdleState");
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        if(movementInput.x != 0 || movementInput.y != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
    }
}
