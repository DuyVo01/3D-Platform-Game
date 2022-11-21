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

    public InAirState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.inputHandler.UsedJump();
        Debug.Log("Enter InAirState");
    }

    public override void Exit()
    {
        base.Exit();
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
            stateMachine.ChangeState(player.idleState);
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
        }   
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();        
        AirMove(player.movementDirection);
        if (player.playerRB.velocity.y < 0 && !isGrounded)
        {
            player.playerRB.velocity += Vector3.up * Physics.gravity.y * (player.fallJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (!isHoldingJump && player.playerRB.velocity.y > 0 && !isGrounded)
        {
            player.playerRB.velocity += Vector3.up * Physics.gravity.y * (player.lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    //private void MovementDirectionWithCamera()
    //{
    //    moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
    //    moveDirection.Normalize();
    //    moveDirection = moveDirection.x * player.cameraTransform.right.normalized + moveDirection.z * player.cameraTransform.forward.normalized;
    //}

    private void AirMove(Vector3 move)
    {
        Vector3 finalMove = move * player.movementSpeed * Time.deltaTime;
        player.playerRB.velocity = new Vector3(finalMove.x, player.currentVelocity.y, finalMove.z);
    }

}
