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
    //public CharacterController controller;

    public float movementSpeed;
    public float rotationSpeed;
    public float jumpHeight;
    public Vector3 currentVelocity;
    public bool isGrounded;
    public float groundCheckRadius;

    public IdleState idleState;
    public MoveState moveState;
    public JumpState jumpState;

    // Start is called before the first frame update
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerRB = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        //controller = GetComponent<CharacterController>();
    }
    void Start()
    {
        stateMachine = new StateMachine();
        idleState = new IdleState(this, stateMachine);
        moveState = new MoveState(this, stateMachine);
        jumpState = new JumpState(this, stateMachine);

        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        currentVelocity = playerRB.velocity;
        stateMachine.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicalUpdate();
    }

    public void PlayerRotation(Vector2 movementInput)
    {
        float targetAngle = Mathf.Atan2(-movementInput.y, movementInput.x) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y; 
        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);
    }

    public void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
