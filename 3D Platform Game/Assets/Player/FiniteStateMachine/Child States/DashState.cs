using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : AbilityState
{
    Vector3 dashDirection;
    float dashStartTime;
    public DashState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
       
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter Dash");
        player.inputHandler.UsedDash();
        dashDirection = player.slopeMovementDirection;
        dashStartTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        player.playerRB.velocity = new Vector3(player.currentVelocity.x, 0f, player.currentVelocity.z);
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        dashDirection = player.slopeMovementDirection;
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
        Dash();
        if(Time.time > dashStartTime + 0.15f)
        {
           isAbilityDone = true;
        }
        
    }

    void Dash()
    {
        Vector3 finalMove = dashDirection * player.dashSpeed * Time.deltaTime;
        player.playerRB.AddForce(finalMove, ForceMode.VelocityChange);
        //player.playerRB.velocity = player.slopeMovementDirection * player.dashSpeed * Time.deltaTime;
        //player.playerRB.velocity = new Vector3(player.currentVelocity.x, 0f, player.currentVelocity.z);
    }
}
   

