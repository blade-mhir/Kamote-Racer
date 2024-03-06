using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] cars;
    public GameObject powerUpPrefab; 

    public List<float> lanePositions = new List<float>() { -4.2f, -1.45f, 1.45f, 4.2f }; 

    public float delayTimer = 1f;
    float timer;
    float powerUpChance = 0.1f;

    void Start()
    {
        timer = delayTimer;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (Random.value < powerUpChance) { 
                // Spawn power-up
                int laneIndex = Random.Range(0, 11);
                Vector3 powerUpPos = new Vector3(lanePositions[laneIndex], transform.position.y, transform.position.z);
                Instantiate(powerUpPrefab, powerUpPos, transform.rotation);  
            } else {
                // Spawn regular enemy car
                int laneIndex = Random.Range(0, 11); // Adjust as needed
                Vector3 enemyPos = new Vector3(lanePositions[laneIndex], transform.position.y, transform.position.z);
                int carNum = Random.Range(0, 19); // Adjust as needed
                Instantiate(cars[carNum], enemyPos, transform.rotation);
            }

            timer = delayTimer; 
        }
    }
}
