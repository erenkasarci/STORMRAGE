using System.Collections;
using System.Collections.Generic;
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


    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
      Movement();
      Jump();
      WallSlide();
      Flip();
    }

    void Movement()
    {
     moveInput = Input.GetAxisRaw("Horizontal");
     rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y); 
    }

    void Jump()
    {
     if(Input.GetKeyDown(KeyCode.W) && isGrounded())
       {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
       }
    }


    private void WallSlide()
    {
      if(!isGrounded() && isWalled())
      {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -5.0f, -5.0f));
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
     return Physics2D.OverlapCircle(wallCheck.position, 0.35f, wallLayer);
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
      Gizmos.DrawWireSphere(wallCheck.position, 0.35f);
    }
}
