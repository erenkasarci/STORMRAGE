using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class enemyPatrol : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceToPoint;
    [SerializeField] private GameObject[] wayPoints;
    private int nextWayPoint = 0;

    void Update()
    {
        Move();
    }

    void Move()
    {
        distanceToPoint = Vector2.Distance(transform.position, wayPoints[nextWayPoint].transform.position);
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[nextWayPoint].transform.position, moveSpeed* Time.deltaTime);

        if(distanceToPoint < 0.2f)
        {
            Rotate();
        }
    }

    void Rotate()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z += wayPoints[nextWayPoint].transform.eulerAngles.z;
        transform.eulerAngles = currentRotation;
        
        ChooseNextWayPoint();
    }

    void ChooseNextWayPoint()
    {
        nextWayPoint++;

        if(nextWayPoint == wayPoints.Length)
        {
            nextWayPoint = 0;
        }
    }
}
