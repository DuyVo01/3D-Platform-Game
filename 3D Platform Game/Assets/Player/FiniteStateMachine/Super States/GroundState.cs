using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : State
{
    public Vector2 movementInput;
    public bool isJumping;
    public GroundState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        movementInput = player.inputHandler.rawMovementInput;
        isJumping = player.inputHandler.isJumping;
        if (isJumping && player.isGrounded)
        {
            stateMachine.ChangeState(player.jumpState);
        }
        
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
    }
}
   

