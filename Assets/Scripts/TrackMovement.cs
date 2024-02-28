using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMovement : MonoBehaviour
{
    public float speed; // Adjust the speed of the movement
    Vector2 offset;

    void Update()
    {
        // Calculate the vertical offset based on time and speed
        offset = new Vector2(0, Time.time * speed);

        // Apply the offset to the material's texture
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
