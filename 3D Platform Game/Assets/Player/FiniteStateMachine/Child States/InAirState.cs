using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAirState : State
{
    private bool isGrounded;
    private bool isHoldingJump;
    private bool isJumping;
    private Vector2 movementInput;
    private Vector3 moveDirection;

    public InAirState(Player player, StateMachine stateMachine, string animationName) : base(player, stateMachine, animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.inputHandler.UsedJump();
        player.playerAnimator.SetBool(animationName, true);
        Debug.Log("Enter InAirState");
    }

    public override void Exit()
    {
        base.Exit();
        player.playerAnimator.SetBool(animationName, false);
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        isGrounded = player.isGrounded;
        movementInput = player.inputHandler.rawMovementInput;
        isHoldingJump = player.inputHandler.isHoldingJump;
        isJumping = player.inputHandler.isJump;

        if (isGrounded && player.currentVelocity.y <= 0.01f)
        {
            stateMachine.ChangeState(player.landState);
        }
        else
        {
            //MovementDirectionWithCamera();

            //rotate the player when moving
            if(movementInput != Vector2.zero)
            {
                player.PlayerRotation(movementInput);
            }  

            if(player.amountOfJumpLeft > 0 && isJumping)
            {
                stateMachine.ChangeState(player.jumpState);
            }

            player.playerAnimator.SetFloat("VelocityY", player.currentVelocity.y);
        }   
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();        
        AirMoveAlternative(player.movementDirection);
        if (player.playerRB.velocity.y < 0 && !isGrounded)
        {
            player.playerRB.velocity += Vector3.up * Physics.gravity.y * (player.fallJumpMultiplier - 1) * Time.deltaTime;
        }
        if (!isHoldingJump && !isGrounded)
        {
            player.playerRB.velocity += Vector3.up * Physics.gravity.y * (player.lowJumpMultiplier - 1) * Time.deltaTime;
        }


    }

    //******************OLD Move Method****************************
    //private void AirMove(Vector3 move)
    //{
    //    Vector3 finalMove = move * player.movementSpeed * Time.deltaTime;
    //    player.playerRB.velocity = new Vector3(finalMove.x, player.currentVelocity.y, finalMove.z);
    //}

    private void AirMoveAlternative(Vector3 move)
    {
        Vector3 finalMove = player.movementSpeed * Time.deltaTime * move;
        //player.playerRB.velocity = new Vector3(finalMove.x, player.currentVelocity.y, finalMove.z);
        Vector3 playerHorizontalVelocity = player.playerRB.velocity;
        playerHorizontalVelocity.y = 0f;
        player.playerRB.AddForce(finalMove - playerHorizontalVelocity, ForceMode.VelocityChange);
    }

}
