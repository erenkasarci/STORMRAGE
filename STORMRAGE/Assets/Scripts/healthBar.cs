using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float lerpSpeed = 0.05f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
      if(healthSlider.value != currentHealth)
      {
        healthSlider.value = currentHealth;
      }
      if(healthSlider.value != easeHealthSlider.value)
      {
        easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
      }
    }

    internal void TakeDamage(float damage)
    {
      currentHealth -= damage;
      if (currentHealth <= 0)
      {
        Die(); 
      }
    }

    void Die()
    {
      Debug.Log("YOU DIED");
    }
}
