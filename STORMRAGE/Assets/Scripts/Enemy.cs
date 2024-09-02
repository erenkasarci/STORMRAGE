using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    //Components
    protected Rigidbody2D EnemyRb;
    protected HealthBar PlayerHealth;

    [Header("STATS")][Space(10)]
     [SerializeField] protected float health;
     [SerializeField] protected float moveSpeed;
     [SerializeField] protected float damage;
     [SerializeField] protected float dmgCD;
     [SerializeField] protected bool canHit;

    protected virtual void GettingComponent()
    {
        EnemyRb = GetComponent<Rigidbody2D>();
        PlayerHealth = GameObject.Find("Health").GetComponent<HealthBar>();
    }

    protected virtual void TakeDamage()
    {

    }

    protected virtual void Die()
    {

    }

    protected virtual void Flip()
    {
        transform.Rotate(new Vector2(0, 180));
    }
    
    protected abstract void Move();

    // Don't Touch Me 
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
      if(collider.gameObject.CompareTag("Player") && canHit)
      {
       PlayerHealth.TakeDamage(-damage);
       
       canHit = false;
       StopCoroutine(DamageCooldown());
       StartCoroutine(DamageCooldown());
      }

      IEnumerator DamageCooldown()
      {
       yield return new WaitForSeconds(dmgCD);
       canHit = true;
      }
      //Will be added Knockback & Immune effect
    }

    protected virtual void Attack1()
    {}

}
