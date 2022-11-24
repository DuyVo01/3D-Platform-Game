using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : AbilityState
{
    Vector3 dashDirection;
    float dashStartTime;
    public DashState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
       
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter Dash");
        player.inputHandler.UsedDash();
        player.playerAnimator.SetBool(animationName, true);
        dashDirection = player.slopeMovementDirection;
        dashStartTime = Time.time;
        
    }

    public override void Exit()
    {
        base.Exit();
        player.playerAnimator.SetBool(animationName, false);
        player.playerAnimator.SetBool("Stop", false);
        player.playerRB.velocity = Vector3.zero;
        //player.playerRB.AddForce(new Vector3(-player.playerRB.velocity.x, 0f, -player.playerRB.velocity.z));
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        dashDirection = player.slopeMovementDirection;
        if (Time.time > dashStartTime + 0.1f)
        {
            player.playerAnimator.SetBool("Stop", true);
            isAbilityDone = true;
        }
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
        Dash();
    }

    void Dash()
    {
        Vector3 finalMove = player.dashSpeed * Time.deltaTime * dashDirection ;
        player.playerRB.AddForce(finalMove, ForceMode.VelocityChange);
    }
}
   

