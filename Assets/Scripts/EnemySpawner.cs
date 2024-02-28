using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject car;
    public float maxPos = 4f;
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
        if (timer <=0) {
                Vector3 enemyPos = new Vector3(Random.Range(-4f, 4f), transform.position.y, transform.position.z);
                    
                Instantiate (car, enemyPos, transform.rotation);
                timer = delayTimer;
        }
         
    }
}