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

    internal void StopTime(float ChangeTime, int RestoreSpeed, float Delay)
    {
      Speed = RestoreSpeed;
      Time.timeScale = ChangeTime; 

      if(Delay > 0)
      {
        StopCoroutine(StartTimeAgain(Delay));
        StartCoroutine(StartTimeAgain(Delay));
      }
      else
      {
        RestoreTime = true;
      }
    }

    IEnumerator StartTimeAgain(float amt)
    {
        RestoreTime = true;
        yield return new WaitForSecondsRealtime(amt);
    }
}
