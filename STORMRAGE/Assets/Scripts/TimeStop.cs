using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private bool RestoreTime;

    void Start()
    {
        RestoreTime = false;
    }

    void Update()
    {
        if(RestoreTime)
        {
            if(Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * Speed;
            }
            else
            {
                Time.timeScale = 1f;
                RestoreTime = false;
            }
        } 
    }

    void StopTime(float ChangeTime, int RestoreSpeed, float Delay)
    {
      Speed = RestoreSpeed;

      if(Delay > 0)
      {
        StopCoroutine(StartTimeAgain(Delay));
        StartCoroutine(StartTimeAgain(Delay));
      }
      else
      {
        RestoreTime = true;
      }

      Time.timeScale = ChangeTime; 
    }

    IEnumerator StartTimeAgain(float amt)
    {
        RestoreTime = true;
        yield return new WaitForSecondsRealtime(amt);
    }
}
