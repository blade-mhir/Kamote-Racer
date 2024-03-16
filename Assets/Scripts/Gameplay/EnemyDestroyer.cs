using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destroy enemy
        }
        else if (collision.gameObject.CompareTag("TurboPowerUp")) 
        {
            Destroy(collision.gameObject); // Destroy powerup
        }
         else if (collision.gameObject.CompareTag("TimeWarpPowerUp")) 
        {
            Destroy(collision.gameObject); // Destroy powerup
        }
        else if (collision.gameObject.CompareTag("ScoreMultiplyPowerUp")) 
        {
            Destroy(collision.gameObject); // Destroy powerup
        }
    }
}
