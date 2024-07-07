using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
  #region Variables
    // Component
    private Rigidbody2D rb;

    // Movement
    [Header("Move Speed")][Space(10)]
    [SerializeField] private float moveSpeed = 8f;
    private float moveInput;

    // Ground Check for Jump
    [Header("Ground Check")][Space(10)]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDistance;
    [SerializeField] private Vector2 boxSize;

    // Wall Check and Slide
    [Header("Wall Slide")][Space(10)]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private bool isWallSliding;

    // Flip
    private bool isFacingRight = true;

    // Jump
    [Header("Jump")][Space(10)]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpTime = 0.25f;
    [SerializeField] private bool isJumping;
    private float jumpTimeCounter;

    // Wall Jump
    [Header("Wall Jump")][Space(10)]
    [SerializeField] private float wallJumpingTime = 0.25f;
    [SerializeField] private float wallJumpingDuration = 0.2f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] private bool isWallJumping;
    private float wallJumpingCounter;

    // Coyote Time 
    [Header("Coyote Time")][Space(10)]
    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
  #endregion

  #region Start
    void Awake()
      {
        rb = GetComponent<Rigidbody2D>();
      }
    void Update()
      {
        Movement();
        Jump();
        WallSlide();
        WallJump();
        Flip();
      
      if(isGrounded())
        {
          coyoteTimeCounter = coyoteTime;
        }
      else
        {
          coyoteTimeCounter -= Time.deltaTime;
        }
      }
  #endregion

    void Movement()
      {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y); 
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

  #region Jumps
    void Jump()
      {
        if(Input.GetKeyDown(KeyCode.W) && coyoteTimeCounter > 0f)
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
    void WallJump()
      {
        if(isWallSliding)
          {
            isWallJumping = false;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
          }
         else
           {
             wallJumpingCounter -= Time.deltaTime;
           }

         if(Input.GetKeyDown(KeyCode.W) && wallJumpingCounter > 0f)
           {
             isWallJumping = true;
             rb.velocity = new Vector2(wallJumpingPower.x, wallJumpingPower.y);
             wallJumpingCounter = 0f;
             Invoke(nameof(StopWallJumping), wallJumpingDuration);
           }
        }
    void StopWallJumping()
      {
        isWallJumping = false;
      }
  #endregion 

  #region Slide
    void WallSlide()
      {
        if(!isGrounded() && isWalled() && Math.Abs(moveInput) > 0.001f )
          {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -4.0f, -4.0f));
            isWallSliding = true;
          }
        else
          {
            isWallSliding = false;
          }
      }
  #endregion

  #region Check Check Methods
    private bool isWalled()
      {
        return Physics2D.OverlapBox(wallCheck.position, boxSize, 90 , wallLayer);
      }
    private bool isGrounded()
      {
        if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, groundDistance, groundLayer))
          {
            return true;
          }
        else
          {
            return false;
          }
      }
  #endregion 

  #region Gizmos
    void OnDrawGizmos()
      {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position-transform.up * groundDistance, boxSize);
        //
        Gizmos.color = Color.green;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(wallCheck.position, Quaternion.Euler(0, 0, 90), Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
        Gizmos.matrix = oldMatrix;
        //
      }
  #endregion
}
