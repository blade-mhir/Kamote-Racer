using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMovement : MonoBehaviour
{
    [System.Serializable] // Make the struct visible in the inspector
    public struct SpeedInterval
    {
        public float startTime;
        public float endTime;
        public float speed;
    }

    public SpeedInterval[] speedIntervals; // Expose this to the Inspector

    Vector2 offset;

    void Update()
    {
        float currentTime = Time.time;

        // Find the correct speed interval
        for (int i = 0; i < speedIntervals.Length; i++)
        {
            if (currentTime >= speedIntervals[i].startTime && currentTime <= speedIntervals[i].endTime)
            {
                // Calculate offset based on the current speed
                offset = new Vector2(0, currentTime * speedIntervals[i].speed);
                GetComponent<Renderer>().material.mainTextureOffset = offset;
                break; // Exit the loop once we've found the interval
            }
        }
    }
}
