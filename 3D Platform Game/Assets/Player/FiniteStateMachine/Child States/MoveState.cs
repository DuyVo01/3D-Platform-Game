using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : GroundState
{
    Vector3 moveDirection;
    public Vector3 slopMoveDirection;
    public MoveState(Player player, StateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter MoveState");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        if (movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else
        {
            //MovementDirectionWithCamera();
            //player.MovementDirRelatedToCameraDir();
            //SlopMoveDirection();    
            player.PlayerRotation(movementInput);
        }  
    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
        if (!player.isOnSlope)
        {
            Move(player.movementDirection);
        }
        else
        {
            SlopeMove();
        }     
    }

    //THIS IS OLD METHOD--------------------------------------------
    //private void MovementDirectionWithCamera()
    //{
    //    moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
    //    moveDirection.Normalize();
    //    moveDirection = moveDirection.x * player.cameraTransform.right.normalized + moveDirection.z * player.cameraTransform.forward.normalized;
    //}

    public void SlopMoveDirection()
    {
        slopMoveDirection = Vector3.ProjectOnPlane(player.movementDirection, player.slopeHit.normal).normalized;
        Debug.DrawRay(player.slopeHit.point, slopMoveDirection, Color.black);
    }

    private void Move(Vector3 move)
    {
        Vector3 finalMove = move * player.movementSpeed * Time.deltaTime;
        player.playerRB.velocity = new Vector3(finalMove.x, 0f, finalMove.z); 
    } 
    private void SlopeMove()
    {
        player.playerRB.velocity = player.slopeMovementDirection * player.movementSpeed * Time.deltaTime;
    }
}
