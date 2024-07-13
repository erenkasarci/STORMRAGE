using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Sliders")][Space(10)]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private float lerpSpeed = 0.05f;

    [Header("Health")][Space(10)]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    

    [Header("Health Potion")][Space(10)]
    [SerializeField] private int maxHealthPotion = 3;
    [SerializeField] private int currentHealthPotion;
    [SerializeField] private float PotionStandTime =0.3f;
    

    void Start()
    {
        currentHealth = maxHealth;
        currentHealthPotion = maxHealthPotion;
    }

    void Update()
    {
      HealUp();

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
      currentHealth += damage;

      if(currentHealth <= 0)
      {
        currentHealth = 0;
        Die(); 
      }
      else if(currentHealth > maxHealth)
      {
        currentHealth = maxHealth;
      }
    }

    void HealUp()
    {
      if(Input.GetKeyDown(KeyCode.E) && currentHealthPotion > 0)
      {
        TakeDamage(20);
        currentHealthPotion -= 1;
        StartCoroutine(HealUsed());
      }
    }

    private IEnumerator HealUsed()
    {
      Color originalColor = playerSprite.color;
      playerSprite.color = Color.red;
      yield return new WaitForSeconds(PotionStandTime);
      playerSprite.color = originalColor;
    }

    void Die()
    {
      Debug.Log("YOU DIED");
    }
}
