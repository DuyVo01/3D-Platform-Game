using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : AbilityState
{
    Vector3 dashDirection;
    float dashStartTime;
    Coroutine ResetDash;

    public DashState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
       
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Enter Dash");

        player.amountOfDashLeft--;

        player.inputHandler.UsedDash();

        player.playerAnimator.SetBool(animationName, true);

        dashDirection = player.slopeMovementDirection;

        dashStartTime = Time.time;

        if(ResetDash != null)
        {
            player.StopCoroutine(ResetDash);
        }
        
        
    }

    public override void Exit()
    {
        base.Exit();

        player.playerAnimator.SetBool(animationName, false);

        player.playerRB.velocity = Vector3.zero;
        
        ResetDash = player.StartCoroutine(DashResetTimer());
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

        if (Time.time > dashStartTime + 0.1f)
        {
            //player.playerRB.AddForce(-player.playerRB.velocity, ForceMode.VelocityChange);

            isAbilityDone = true;
        }
        
    }

    void Dash()
    {
        Vector3 finalMove = player.dashSpeed * Time.deltaTime * dashDirection ;

        player.playerRB.AddForce(finalMove, ForceMode.VelocityChange);
    }

    IEnumerator DashResetTimer()
    {
        if(player.amountOfDashLeft == 0)
        {
            while (player.amountOfDashLeft < 2)
            {
                yield return new WaitForSeconds(1f);

                player.amountOfDashLeft = 2;
            }
        } else if(player.amountOfDashLeft == 1)
        {
            while (player.amountOfDashLeft < 2)
            {
                yield return new WaitForSeconds(0.1f);

                player.amountOfDashLeft = 2;
            }
        }
        
    }

    
}
   

