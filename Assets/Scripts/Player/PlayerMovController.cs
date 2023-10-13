using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovController : MonoBehaviour
{
    public PlayerInput controls;
    private Rigidbody rb;
    private CapsuleCollider playerCapsule;
    private BoxCollider groundBox;
    public PlayerAttack playerAttack;

    [Header("Movement")]
    public float acceleration;
    public float topSpeed;
    public float deceleration;
    float targetVelocity;
    private float currentSpeed = 0f;
    [SerializeField] bool isFacingRight = true;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpTime;
    private bool isJumping;
    public bool canJump = true;
    private bool isFalling;
    private float jumpTimeCounter;
    public float airSpeedMultiplier = 1f;
    public float descentSpeed;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.81f;

    [Header("Animation")]
    public Animator animator;

    [Header("Ground Check")]
    public bool isGrounded;
    public GroundChecker groundChecker;

    [Header("Debug")]
    [SerializeField] public Vector2 direction;
    [SerializeField] private float action;






    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCapsule = GetComponent<CapsuleCollider>();

        rb.useGravity = false;

    }



    private void Update()
    {
        if (playerAttack.canMove)
        {
            rb.constraints = RigidbodyConstraints.None;  
            rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;  
            rb.constraints &= ~RigidbodyConstraints.FreezeRotation;

        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;

        }

        Jump(); 
        Animation();
    }

    private void FixedUpdate()
    {

        Move();
        
        Gravity();
        Descend();
    }



    private void Gravity()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    #region Movement

    private void Move()
    {
        
        float moveX = UserInput.instance.moveInput.x;
        if (isGrounded)
        {
             targetVelocity = moveX * topSpeed;
        }
        else
        {
             targetVelocity = moveX * topSpeed * airSpeedMultiplier;
        }

            // Calculate the difference between the target velocity and current velocity.
            float velocityChange = targetVelocity - currentSpeed;

            // Apply acceleration and deceleration.
            float accelerationAmount = moveX != 0f ? acceleration : deceleration;
            float change = accelerationAmount * Time.deltaTime;

            if (Mathf.Abs(velocityChange) > change)
            {
                velocityChange = Mathf.Sign(velocityChange) * change;
            }

            // Update the current speed.
            currentSpeed += velocityChange;

            // Apply movement to the Rigidbody2D.
            Vector2 movement = new Vector2(currentSpeed, rb.velocity.y);
            rb.velocity = movement;

            

            if (!isFacingRight && movement.x > 0f)
            {
                Flip();
            }
            else if (isFacingRight && movement.x < 0f)
            {
                Flip();
            }
        
            

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    #endregion

    #region Jumping
    private void Jump()
    {
        if (UserInput.instance.controls.Player.Jump.WasPressedThisFrame() && (isGrounded || groundChecker.isCoyote))
        {
            isJumping = true;

            

            groundChecker.isCoyote = false;
            jumpTimeCounter = jumpTime;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            
        }

        if (UserInput.instance.controls.Player.Jump.IsPressed())
        {
            groundChecker.isCoyote = false;
            
            if (jumpTimeCounter > 0 && isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
                
            }
            else
            {
                isJumping = false;
                
            }

        }

        if (UserInput.instance.controls.Player.Jump.WasReleasedThisFrame())
        {

            isJumping = false;
            
            
        }
    }
    #endregion

    #region Descend
    private void Descend()
    {
        if (!isGrounded)
        {
            float moveDown = UserInput.instance.moveInput.y;

            if (moveDown == -1)
            {
                Debug.Log("Desdend");

                isJumping = false;

                Vector3 descendVector = descentSpeed * gravityScale * globalGravity * Vector3.up;

                rb.AddForce(descendVector, ForceMode.Impulse);

                

            }

        }
    }

    #endregion

    #region Animation

    private void Animation()
    {
        //check if player is moving on the X axis, for the animation
        float movementSpeed = Mathf.Abs(rb.velocity.x);

        //check if the player is grounded for the movement animation
        if (isGrounded)
        {
            
            animator.SetFloat("Speed", Mathf.Abs(movementSpeed));
        }

        //check if the player is on air for the jump animation
        if (!isGrounded)
        {

            animator.SetBool("isJumping", true);

            Debug.Log("Jump");
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetFloat("Speed", Mathf.Abs(movementSpeed));
        }


    }

    #endregion
}