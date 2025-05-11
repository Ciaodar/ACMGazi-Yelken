using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class GencTilkiController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float airWalkSpeed = 10f;
    public float jumpImpulse = 10f;
    public float effect = 1f;
    private bool justJumped = false;
    public bool isUnderForce = false;
    public float forceSure = 1f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    
   

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving)
                {
                    if (IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                        return MathF.Abs(rb.velocity.x); //airwalk speed is equal to the value before jump.
                    }
                  
                }
                else
                {
                    return 0;              
                }                                
            }
            else
            {
                return 0;
            }
        
        }
    }
    
    public bool CanMove {
        get
        {
            return animator.GetBool("canMove");
        }

    }
    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }
    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool("isRunning", value);
        }
    }
    
    [SerializeField]
    private bool _isGrounded;
    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            _isGrounded = value;
            animator.SetBool("isGrounded", value);
        }
    }
    
    public bool IsAlive
    {
        get
        {
            return animator.GetBool("isAlive");
        }
    }
    
    private bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(!isUnderForce)
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed * effect , rb.velocity.y);
        
        animator.SetFloat("yVelocity", rb.velocity.y);
    }
    
    private IEnumerator ResetForceState()
    {
        yield return new WaitForSeconds(forceSure);
        isUnderForce = false;
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        { 
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded && CanMove)
        {
            animator.SetTrigger("jumpTrigger");
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            justJumped = true;
            IsGrounded = false;
        }
    }
}
