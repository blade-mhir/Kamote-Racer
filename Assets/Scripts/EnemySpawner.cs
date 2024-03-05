using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] cars;
    public float[] lanePositions = { -4.2f, -1.45f, 1.45f, 4.2f }; // Define your lane positions
    public float delayTimer = 0.5f;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = delayTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            int laneIndex = Random.Range(0, 11); // Randomly select a lane
            Vector3 enemyPos = new Vector3(lanePositions[laneIndex], transform.position.y, transform.position.z);

            int carNum = Random.Range(0, 20); // Adjust as needed
            Instantiate(cars[carNum], enemyPos, transform.rotation);

            timer = delayTimer;
        }
    }
}
