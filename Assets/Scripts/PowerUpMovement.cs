using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    public float speed; 

    void Update()
    {
        // Use the same movement logic as your enemy cars
        transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle what happens when the player's car collides with an enemy
            Destroy(gameObject);
        }
    }
}
