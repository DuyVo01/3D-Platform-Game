using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AbilityState
{
    public JumpState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter JumpState");
        player.amountOfJumpLeft--;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
        Jump();
        isAbilityDone = true;
    }

    private void Jump()
    {
        player.playerRB.velocity = new Vector3(player.currentVelocity.x, 0f, player.currentVelocity.z);
        player.playerRB.velocity += Vector3.up * player.jumpHeight;
    }

    public void ResetJumpCount() => player.amountOfJumpLeft = 2;
}
