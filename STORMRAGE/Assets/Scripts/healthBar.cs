using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameObject player;

    [Header("Sliders")][Space(10)]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;
    [SerializeField] private float lerpSpeed = 0.05f;

    [Header("Health")][Space(10)]
    [SerializeField] internal float maxHealth = 100f;
    [SerializeField] internal float currentHealth;
    

    [Header("Health Potion")][Space(10)]
    [SerializeField] internal int maxHealthPotion = 3;
    [SerializeField] internal int currentHealthPotion;
    [SerializeField] private float potionStandTime =0.8f;
    [SerializeField] private float healthAmount = 20.0f;
    
    void Awake()
    {
      playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
      
    }

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
      if(Input.GetKeyDown(KeyCode.E) && currentHealthPotion > 0 && playerMovement.isGrounded())
      {
        TakeDamage(healthAmount);
        currentHealthPotion -= 1;
        StartCoroutine(playerMovement.StandTime(potionStandTime));
        StartCoroutine(HealUsed());   
      }
    }

    private IEnumerator HealUsed()
    {
      Color originalColor = playerSprite.color;
      playerSprite.color = Color.red;
      yield return new WaitForSeconds(potionStandTime);
      playerSprite.color = originalColor;
    }

    void Die()
    {
      Destroy(player);
    }
}
