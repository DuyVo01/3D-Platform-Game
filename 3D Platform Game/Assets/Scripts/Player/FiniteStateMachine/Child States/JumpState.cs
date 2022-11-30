using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AbilityState
{
    public JumpState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter JumpState");
        player.playerAnimator.SetBool(animationName, true);
        player.amountOfJumpLeft--;
    }

    public override void Exit()
    {
        base.Exit();
        player.playerAnimator.SetBool(animationName, false);
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
        player.playerRB.velocity = Vector3.zero;
        Vector3 jumpDirection = new Vector3(player.movementDirection.x * player.movementSpeed * 0.1f * Time.deltaTime, player.jumpHeight , player.movementDirection.z * player.movementSpeed * 0.1f * Time.deltaTime);

        player.playerRB.AddForce(jumpDirection, ForceMode.VelocityChange);
    }

    public void ResetJumpCount() => player.amountOfJumpLeft = 2;
}
