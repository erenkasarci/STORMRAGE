using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 20.0f;
    [SerializeField] private float damageAmount = 1.0f;
    [SerializeField] private float damageInterval = 0.2f;
    private HealthBar health;
    private Coroutine damageCoroutine;

    void Awake()
    {
      health = GameObject.Find("Health").GetComponent<HealthBar>();
    }

    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      if(other.gameObject.CompareTag("Player"))
      {
        damageCoroutine = StartCoroutine(DealDamageOverTime());
      }
    }

    void OnTriggerExit2D(Collider2D other)
    {
      StopCoroutine(damageCoroutine);
      damageCoroutine = null;
    }

    private IEnumerator DealDamageOverTime()
    {
      while(true)
      {
        health.TakeDamage(damageAmount);
        yield return new WaitForSeconds(damageInterval);
      }
    }
}
