using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] cars;
    public PowerUpData[] powerUpPrefabs;

    public List<float> lanePositions = new List<float>() { -4.2f, -1.45f, 1.45f, 4.2f }; 
    public float delayTimer = 1f;
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
            if (Random.value < GetTotalPowerUpChance()) { 
                SpawnRandomPowerUp();
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

    void SpawnRandomPowerUp() 
    {
        float randomValue = Random.value * GetTotalPowerUpChance();
        float cumulativeChance = 0.0f;

        for (int i = 0; i < powerUpPrefabs.Length; i++) {
            cumulativeChance += powerUpPrefabs[i].spawnChance;
            if (randomValue <= cumulativeChance) {
                // Spawn this power-up
                int laneIndex = Random.Range(0, lanePositions.Count); // Change if needed
                Vector3 powerUpPos = new Vector3(lanePositions[laneIndex], transform.position.y, transform.position.z);
                Instantiate(powerUpPrefabs[i].prefab, powerUpPos, transform.rotation);
                break; // Spawned, so exit the loop
            }
        }
    }

    float GetTotalPowerUpChance() {
        float totalChance = 0.0f;
        foreach (PowerUpData powerup in powerUpPrefabs) {
            totalChance += powerup.spawnChance;
        }
        return totalChance;
    }
}

[System.Serializable]
public struct PowerUpData 
{
    public GameObject prefab;
    public float spawnChance;
}
