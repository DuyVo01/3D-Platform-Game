using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    StateMachine stateMachine;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask whatIsGround;

    public InputHandler inputHandler;
    public Rigidbody playerRB;
    public Transform cameraTransform;
    public CapsuleCollider playerCapsuleCollider;
    

    public float movementSpeed;
    public Vector3 movementDirection;
    public Vector3 slopeMovementDirection;
    public float rotationSpeed;
    public float dashSpeed;

    //Jump Variables
    public float jumpHeight;
    public float holdingJumpHeight;
    public float fallJumpMultiplier;
    public float lowJumpMultiplier;
    public int amountOfJumpLeft;

    public Vector3 currentVelocity;
    public bool isGrounded;
    public bool isOnSlope;
    public float groundCheckRadius;
    public RaycastHit slopeHit;

    public IdleState idleState;
    public MoveState moveState;
    public JumpState jumpState;
    public InAirState inAirState;
    public DashState dashState;

    // Start is called before the first frame update
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerRB = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        //controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        stateMachine = new StateMachine();
        idleState = new IdleState(this, stateMachine);
        moveState = new MoveState(this, stateMachine);
        jumpState = new JumpState(this, stateMachine);
        inAirState = new InAirState(this, stateMachine);
        dashState = new DashState(this, stateMachine);

        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        OnSlope();
        MovementDirRelatedToCameraDir();
        SlopMoveDirection();
        currentVelocity = playerRB.velocity;
        stateMachine.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicalUpdate();
    }

    public void MovementDirRelatedToCameraDir()
    {
        movementDirection = new Vector3(inputHandler.rawMovementInput.x, 0f, inputHandler.rawMovementInput.y);
        movementDirection.Normalize();
        movementDirection = movementDirection.x * cameraTransform.right.normalized + movementDirection.z * cameraTransform.forward.normalized;
        movementDirection.y = 0f;
    }

    public void SlopMoveDirection()
    {
        slopeMovementDirection = Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;
        Debug.DrawRay(slopeHit.point, slopeMovementDirection, Color.black);
    }

    public void PlayerRotation(Vector2 movementInput)
    {
        float targetAngle = Mathf.Atan2(-movementInput.y, movementInput.x) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);
    }

    public void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public void OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerCapsuleCollider.height / 2 + 0.5f))
        {
            Debug.DrawRay(slopeHit.point, slopeHit.normal, Color.red);
            if(slopeHit.normal != Vector3.up)
            {
                isOnSlope = true;
            }
            else
            {
                isOnSlope = false;
            }
        }
        else
        {
            isOnSlope = false;
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
