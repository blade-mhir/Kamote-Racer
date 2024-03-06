using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUps; 
    public float[] lanePositions = { -4.2f, -1.45f, 1.45f, 4.2f }; 
    public float delayTimer = 2.0f; 
    float timer;

    void Start()
    {
        timer = delayTimer;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            int laneIndex = Random.Range(0, lanePositions.Length); 
            int powerUpIndex = Random.Range(0, powerUps.Length); 

            Vector3 powerUpPos = new Vector3(lanePositions[laneIndex], transform.position.y, transform.position.z);
            Instantiate(powerUps[powerUpIndex], powerUpPos, transform.rotation);

            timer = delayTimer;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            ApplyPowerUp(other.gameObject); 
            Destroy(gameObject);  
        }
    }

    void ApplyPowerUp(GameObject player)
    {
        // Implement your power-up logic here
     
        }
}
