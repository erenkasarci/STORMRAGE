using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class FatFly : Enemy
{
    private bool touchedRoof, touchedWall, touchedGround;
    [SerializeField] private bool goingUp = true;
    [SerializeField] protected Vector2 moveDirection = new Vector2(1f, 2f);

    [Header("Checkers")][Space(10)]
    [SerializeField] private GameObject roofCheck;
    [SerializeField] private GameObject wallCheck;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private Vector2 roofCheckSize, wallCheckSize, groundCheckSize;
    [SerializeField] LayerMask groundLayer;

    void Awake()
    {
        GettingComponent();
    }

    void Update()
    {
      Move();
    }

    protected override void Move()
    {
       EnemyRb.velocity = moveDirection * moveSpeed;
       //
       touchedRoof = HitDedector(roofCheck, roofCheckSize, groundLayer);
       touchedWall = HitDedector(wallCheck, wallCheckSize, groundLayer);
       touchedGround = HitDedector(groundCheck, groundCheckSize, groundLayer);

       if(touchedWall)
       {
        Flip();
       }
       if(touchedRoof && goingUp)
       {
        ChangeYDirection();
       }
       if(touchedGround && !goingUp)
       {
        ChangeYDirection();
       }
    }

    private bool HitDedector(GameObject gameObject, Vector2 size, LayerMask layer)
    {
        return Physics2D.OverlapBox(gameObject.transform.position, size, 0f, layer);
    }

    protected override void Flip()
    {
        base.Flip();
        moveDirection.x = -moveDirection.x;
    }

    protected void ChangeYDirection()
    {
        moveDirection.y = -moveDirection.y;
        goingUp = !goingUp;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        //Extra will be added Knockback & Immune effect
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize);
        Gizmos.DrawWireCube(roofCheck.transform.position, roofCheckSize);
        Gizmos.DrawWireCube(wallCheck.transform.position, wallCheckSize);
    }

}
