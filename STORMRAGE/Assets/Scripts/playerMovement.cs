using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 8f;
    public float jumpForce = 0.001f;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Transform wallCheck;
    public bool isWallSliding;
    //public float wallSlidingSpeed = 5.0f;
    private float moveInput;
    private bool isFacingRight = true;
    public bool isJumping;
    private float jumpTimeCounter;
    public float jumpTime;

    public bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter;
    private bool isGroundedResult;



    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
      Movement();
      Jump();
      WallSlide();
      WallJump();

      if (!isWallJumping)
        {
            Flip();
        }

      if (isGrounded())
      {
        coyoteTimeCounter = coyoteTime;
      }
      else
      {
        coyoteTimeCounter -= Time.deltaTime;
      }

    }


    void Movement()
    {
     moveInput = Input.GetAxisRaw("Horizontal");
     rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y); 
    }

    void Jump()
    {
     if(Input.GetKeyDown(KeyCode.W) && coyoteTimeCounter > 0f )
       {
        isJumping = true;
        jumpTimeCounter = jumpTime;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
       }

     else if(Input.GetKey(KeyCode.W) && isJumping == true)
        {
          if(jumpTimeCounter > 0)
           {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
           }
           else
           {
            isJumping = false;
           }
        }
     else if(Input.GetKeyUp(KeyCode.W))
     {

      isJumping = false;
      coyoteTimeCounter = 0f;
     }
    }

        private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void StopWallJumping()
    {
      isWallJumping = false;
    }

    void WallSlide()
    {
      if(!isGrounded() && isWalled() && moveInput != 0f)
      {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -4.0f, -4.0f));
        isWallSliding = true;
      }
      else
      {
        isWallSliding = false;
      }
    }



     public bool isGrounded()
    {
      if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
      {
        return true;
      }
      else
      {
        return false;
      }
    }


    public bool isWalled()
    {
     return Physics2D.OverlapBox(wallCheck.position, boxSize, 90 , wallLayer);
    }
    

    void Flip()
    {
      if(isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
      {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
      }
    }
    
    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
      Gizmos.color = Color.green;
      Matrix4x4 oldMatrix = Gizmos.matrix;
      Matrix4x4 rotationMatrix = Matrix4x4.TRS(wallCheck.position, Quaternion.Euler(0, 0, 90), Vector3.one);
      Gizmos.matrix = rotationMatrix;
      Gizmos.DrawWireCube(Vector3.zero, boxSize);
      Gizmos.matrix = oldMatrix;
    }
}
