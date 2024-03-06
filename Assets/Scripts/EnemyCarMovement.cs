using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarMovement : MonoBehaviour
{
    [System.Serializable]
    public struct SpeedInterval
    {
        public float startTime;
        public float endTime;
        public float speed;
    }

    public SpeedInterval[] speedIntervals;
    public float initialSpeed; // Set an initial speed in case there are no intervals

    private float currentSpeed;

    void Start()
    {
        currentSpeed = initialSpeed;  // Default to the initial speed

        // Find the initial speed interval if any are defined
        if (speedIntervals.Length > 0) 
        {
            UpdateCurrentSpeed(); 
        }
    }

    void Update()
    {
        // Check if it's time to update the speed interval
        UpdateCurrentSpeed(); 

        transform.Translate(new Vector3(0, 1, 0) * currentSpeed * Time.deltaTime);
    }

    void UpdateCurrentSpeed() 
    {
        for (int i = 0; i < speedIntervals.Length; i++)
        {
            if (Time.time >= speedIntervals[i].startTime && Time.time <= speedIntervals[i].endTime)
            {
                currentSpeed = speedIntervals[i].speed;
                break; // Speed found for this interval
            }
        }
    }
}
