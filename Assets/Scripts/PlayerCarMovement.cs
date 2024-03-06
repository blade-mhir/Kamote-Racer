using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarMovement : MonoBehaviour
{
    [System.Serializable]
    public struct SpeedInterval
    {
        public float startTime;
        public float endTime;
        public float speed;
    }

    public SpeedInterval[] speedIntervals;
    public float minX = -4.5f;
    public float maxX = 4.5f;
    public Vector3 respawnPosition;
    public float invulnerabilityDuration = 1f;

    private Rigidbody2D rb;
    [SerializeField] private int livesRemaining = 3;
    private float currentSpeed; 

    public GameObject explosionEffectPrefab;
    public GameObject respawnEffectPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        respawnPosition = transform.position; 

        currentSpeed = speedIntervals[0].speed; // Set initial speed
    }

    void Update()
    {
        // Find the correct speed interval
        for (int i = 0; i < speedIntervals.Length; i++)
        {
            if (Time.time >= speedIntervals[i].startTime && Time.time <= speedIntervals[i].endTime)
            {
                currentSpeed = speedIntervals[i].speed;
                break; 
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * currentSpeed, 0f); 

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            // Handle what happens when the player's car collides with a Power Up
            // ... 
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            livesRemaining--;

            if (livesRemaining > 0)
            {
                Respawn();  
                StartCoroutine(InvulnerabilityPeriod());
            }
            else 
            {
                if (explosionEffectPrefab != null) 
                {
                    GameObject explosionInstance = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(explosionInstance, 0.5f); // Adjust the delay '2f' as needed
                }
                gameObject.SetActive(false);
                // Game Over! 
                Debug.Log("Game Over"); 
                // You'll likely want to disable the player car or handle game over logic here
            }
        }
    }

    void Respawn()
    {
        StartCoroutine(RespawnWithDelay()); 
    }

    IEnumerator RespawnWithDelay() 
    {
        if (respawnEffectPrefab != null)
        {
            GameObject respawnInstance = Instantiate(respawnEffectPrefab, respawnPosition, Quaternion.identity);
            Destroy(respawnInstance, 1f); // Adjust the delay as needed

            yield return new WaitForSeconds(1f); // Wait for the animation to play
        }

        // Now respawn the player
        transform.position = respawnPosition;
        gameObject.SetActive(true); 
    }


    IEnumerator InvulnerabilityPeriod()
    {
        // Make car temporarily invulnerable (e.g., by disabling its collider)
        GetComponent<Collider2D>().enabled = false; 

        yield return new WaitForSeconds(invulnerabilityDuration);

        // Re-enable the collider
        GetComponent<Collider2D>().enabled = true; 
    }
}
