using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingPlace : MonoBehaviour
{
    [SerializeField] private bool isResting;
    [SerializeField] private float distanceToBonfire;
    [SerializeField] private GameObject player;
    private PlayerMovement playerMovement;
    private HealthBar health;
    
    void Start()
    {
      health = GameObject.Find("Health").GetComponent<HealthBar>();
      playerMovement =  player.GetComponent<PlayerMovement>();  
    }

    void Update()
    {
      Rest(); 
    }

    void Rest()
    {
        distanceToBonfire = Vector2.Distance(transform.position, player.transform.position);

        if(distanceToBonfire < 1.4f)
        {
          if(Input.GetKeyDown(KeyCode.F))
          {
            isResting = true;
            playerMovement.SetCanMove(false);
          }
          if(Input.GetKeyDown(KeyCode.Escape))
          {
            isResting = false;
            playerMovement.SetCanMove(true); 
          }
        }

        if(isResting)
        {
          health.currentHealth = health.maxHealth;
          health.currentHealthPotion = health.maxHealthPotion;
        }
    }
}
