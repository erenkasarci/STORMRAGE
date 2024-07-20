using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatFlyEnemy : MonoBehaviour
{
    private bool goingUp = true;
    private bool touchedRoof, touchedWall, touchedGround;
    //Components
    private Rigidbody2D EnemyRb;
    private HealthBar PlayerHealth;
    private TimeStop timeStop;

    [Header("DMG")][Space(10)]
    [SerializeField] private float DealDamage = 20;
    [SerializeField] private float dmgCooldown = 0.5f;
    [SerializeField] private bool canHit = true;

    [Header("Movement")][Space(10)]
    [SerializeField] private Vector2 moveDirection = new Vector2(1f, 2f);
    [SerializeField] private float moveSpeed = 0.5f;

    [Header("Checkers")][Space(10)]
    [SerializeField] private GameObject roofCheck;
    [SerializeField] private GameObject wallCheck;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private Vector2 roofCheckSize, wallCheckSize, groundCheckSize;
    [SerializeField] LayerMask groundLayer;

    void Awake()
    {
        EnemyRb = GetComponent<Rigidbody2D>();
        PlayerHealth = GameObject.Find("Health").GetComponent<HealthBar>();
        timeStop = GameObject.Find("Game Manager").GetComponent<TimeStop>();
    }

    void Update()
    {
        EnemyRb.velocity = moveDirection * moveSpeed;
        HitLogic();
    }

    void HitLogic()
    {
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

    void ChangeYDirection()
    {
        moveDirection.y = -moveDirection.y;
        goingUp = !goingUp;
    }

    void Flip()
    {
        transform.Rotate(new Vector2(0, 180));
        moveDirection.x = -moveDirection.x;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && canHit)
        {
          PlayerHealth.TakeDamage(-DealDamage);
          canHit = false;
          timeStop.StopTime(0, 6, 0);
          StopCoroutine(DamageCooldown());
          StartCoroutine(DamageCooldown());
        }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(dmgCooldown);
        canHit = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize);
        Gizmos.DrawWireCube(roofCheck.transform.position, roofCheckSize);
        Gizmos.DrawWireCube(wallCheck.transform.position, wallCheckSize);
    }
}
