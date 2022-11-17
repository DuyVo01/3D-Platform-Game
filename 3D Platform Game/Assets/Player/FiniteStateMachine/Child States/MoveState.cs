using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : GroundState
{
    Vector3 move;
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
        player.playerRB.velocity = Vector3.zero;
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
        if (movementInput.x == 0 && movementInput.y == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        move = new Vector3(movementInput.x, 0, movementInput.y);
        move = move.x * player.cameraTransform.right.normalized + move.z * player.cameraTransform.forward.normalized;
        move.y = 0f;

    }

    public override void PhysicalUpdate()
    {
        base.PhysicalUpdate();
        player.PlayerRotation(movementInput);
        Move(move);
        
    }

    public void Move(Vector3 move)
    {
        Vector3 finalMove = move * player.movementSpeed * Time.deltaTime;
        player.playerRB.velocity = new Vector3(finalMove.x, player.currentVelocity.y, finalMove.z);
    }
}
