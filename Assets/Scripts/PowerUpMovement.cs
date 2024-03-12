using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
[System.Serializable]
    public struct SpeedInterval
    {
        public float startTime;
        public float endTime;
        public float speed;
    }

    public SpeedInterval[] speedIntervals;
    private float currentSpeed;

    void Start()
    {
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle what happens when the player's car collides with an enemy
            Destroy(gameObject);
        }
    }

    public void UpdateSpeedForTimeWarp(float slowdownFactor)
    {
    currentSpeed *= slowdownFactor;
    }
}
