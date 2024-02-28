using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarMovement : MonoBehaviour
{
    public float carSpeed = 5f; 
    public float minX = -4.5f; // Left boundary
    public float maxX = 4.5f;  // Right boundary

    private Vector3 position;

    void Start() 
    {
        position = transform.position; 
    }

    void Update()
    {
        position.x += Input.GetAxis("Horizontal") * carSpeed * Time.deltaTime;

        // Clamp the position within limits
        position.x = Mathf.Clamp(position.x, minX, maxX);

        transform.position = position;
    }
}
