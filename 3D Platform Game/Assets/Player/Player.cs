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
    public Animator playerAnimator;
    
    //Ground and Rotation
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
    public bool animationFinished;
    public float groundCheckRadius;
    public RaycastHit slopeHit;

    //Floating
    public RaycastHit rayFloatHit;
    public float floatingDistance;
    public float characterOffsetY;

    public IdleState idleState;
    public MoveState moveState;
    public JumpState jumpState;
    public InAirState inAirState;
    public DashState dashState;
    public LandState landState;

    // Start is called before the first frame update
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerRB = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        playerAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        stateMachine = new StateMachine();
        idleState = new IdleState(this, stateMachine, "Idle");
        moveState = new MoveState(this, stateMachine, "Moving");
        jumpState = new JumpState(this, stateMachine, "InAir");
        inAirState = new InAirState(this, stateMachine, "InAir");
        dashState = new DashState(this, stateMachine, "Dashing");
        landState = new LandState(this, stateMachine, "Landing");

        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        OnSlope();
        MovementDirRelatedToCameraDir();
        SlopMoveDirection();
        //FloatingCollider();
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
        float targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);
    }

    public void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public void OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerCapsuleCollider.height + 0.5f))
        {
            //Debug.DrawRay(transform.position, slopeHit.normal, Color.red);
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

    public void FloatingCollider()
    {
        float rideHeight = playerCapsuleCollider.center.y + floatingDistance;
        Vector3 desiredPosition = transform.position;
        Ray downRay = new Ray(transform.TransformPoint(playerCapsuleCollider.center), Vector3.down);
        bool rayDidHit = Physics.Raycast(downRay, out rayFloatHit, rideHeight, whatIsGround);
        if (rayDidHit)
        {
            Debug.DrawRay(transform.TransformPoint(playerCapsuleCollider.center), downRay.direction, Color.yellow);
            float distanceToLift = playerCapsuleCollider.center.y - rayFloatHit.distance ;
            //if(distanceToLift == 0)
            //{
            //    return;
            //}
            float amountToLift = distanceToLift * 25f - playerRB.velocity.y;

            Vector3 springForce = new Vector3(0f, amountToLift, 0f);
             
            playerRB.AddForce(springForce, ForceMode.VelocityChange);
        }

        
        
    }

    //public void FloatingCollider()
    //{
    //    float rideHeight = playerCapsuleCollider.height / 2 + floatingDistance;
    //    Ray downRay = new Ray(transform.position, Vector3.down);
    //    bool rayDidHit = Physics.Raycast(downRay, out rayFloatHit, rideHeight);
    //    if (rayDidHit)
    //    {
    //        //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + characterOffsetY, transform.position.z), downRay.direction, Color.yellow);
    //        Vector3 vel = playerRB.velocity;
    //        Vector3 rayDir = downRay.direction;

    //        Vector3 otherVel = Vector3.zero;
    //        Rigidbody hitBody = rayFloatHit.rigidbody;
    //        if (hitBody != null)
    //        {
    //            otherVel = hitBody.velocity;
    //        }

    //        float rayDirVel = Vector3.Dot(rayDir, vel);
    //        float otherDirVel = Vector3.Dot(rayDir, otherVel);

    //        float relVel = rayDirVel - otherDirVel;

    //        float x = rayFloatHit.distance - rideHeight;

    //        float springForce = (x * 5f) - (relVel * 5f);

    //        Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.yellow);

    //        playerRB.AddForce(rayDir * springForce);

    //        if (hitBody != null)
    //        {
    //            hitBody.AddForceAtPosition(rayDir * -springForce, rayFloatHit.point);
    //        }
    //    }
    //}


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void AnimationFinished()
    {
        animationFinished = true;
    }
}
