using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

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
    [SerializeField] public int livesRemaining = 3;
 public int GetLivesRemaining() 
    {
        return livesRemaining; 
    }

public GameOverMenu gameOverMenu;

    private float currentSpeed; 
    public GameObject explosionEffectPrefab;
    public GameObject respawnEffectPrefab;

//TURBO BOOST PU
    [Header("Turbo Boost")]
    public int powerUpsNeededForTurbo = 3; 
    public float turboBoostDuration = 5f;
    public float turboBoostSpeedMultiplier = 2f; 

    private int powerUpsCollected = 0; 
    private bool isTurboActive = false;
    private float originalSpeed; 

//HEALTH PU
    [Header("Health Power-Up")]
    public int powerUpsNeededForHealth = 1; // You can set to 1 for immediate effect
    public int maxLives = 3; // Maximum lives for the car
    private int healthPowerUpsCollected = 0; 

// TIME WARP POWER-UP
    [Header("Time Warp")]
    public int powerUpsNeededForTimeWarp = 3; 
    public float timeWarpDuration = 5f;
    public float timeWarpSlowdownFactor = 0.5f;

    private int timeWarpPowerUpsCollected = 0; 
    private bool isTimeWarpActive = false;

    // References to other scripts
    public TrackMovement trackMovement;
    public EnemyCarMovement[] enemyMovements; // Array for multiple enemies
    public PowerUpMovement[] powerUpMovements; // Array for multiple power-ups

    [Header("Jet Fighter Power-Up")]
    public int jetFighterPowerUpsNeeded = 3; 
    private int jetFighterPowerUpsCollected = 0; 

    public ScoreManager scoreManager;
    private float timeSurvived = 0f;

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
        if (isTurboActive) 
        {
            currentSpeed = originalSpeed * turboBoostSpeedMultiplier;
        }
        else
        {
        for (int i = 0; i < speedIntervals.Length; i++)
        {
            if (Time.time >= speedIntervals[i].startTime && Time.time <= speedIntervals[i].endTime)
            {
                currentSpeed = speedIntervals[i].speed;
                break; 
            }
        }
        }
        // Find the correct speed interval
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * currentSpeed, 0f); 

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        timeSurvived += Time.deltaTime; 
        int score = (int)timeSurvived * 10; 
        scoreManager.UpdateScore(score); 

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TimeWarpPowerUp"))
        {
            timeWarpPowerUpsCollected++;

            if (timeWarpPowerUpsCollected >= powerUpsNeededForTimeWarp)
            {
                timeWarpPowerUpsCollected = 0; 
                ActivateTimeWarp();
            }

            Destroy(collision.gameObject); 
        }
        else if (collision.gameObject.CompareTag("JetFighterPowerUp"))
        {
            Debug.Log("Collided with Jet Fighter power-up!"); 
            jetFighterPowerUpsCollected++; 
            Debug.Log("jetFighterPowerUpsCollected: " + jetFighterPowerUpsCollected); // Changed to the correct variable
            if (jetFighterPowerUpsCollected >= jetFighterPowerUpsNeeded) 
            {
                Debug.Log("Ready to activate Jet Fighter!");
                jetFighterPowerUpsCollected = 0; // Reset the counter
                StartCoroutine(ActivateJetFighter()); 
            }

            Destroy(collision.gameObject); 
        }
        else if (collision.gameObject.CompareTag("TurboPowerUp"))
        {
            powerUpsCollected++;

            if (powerUpsCollected >= powerUpsNeededForTurbo)
            {
                powerUpsCollected = 0; // Reset the counter
                ActivateTurboBoost();
            }
            // Destroy the power-up GameObject
            Destroy(collision.gameObject); 
        }
        else if (collision.gameObject.CompareTag("HealthPowerUp"))
        {
            healthPowerUpsCollected++;

            if (healthPowerUpsCollected >= powerUpsNeededForHealth)
            {
                healthPowerUpsCollected = 0; // Reset the counter
                ReplenishLife();
            }

            Destroy(collision.gameObject); 
        } 
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            livesRemaining--;

            if (livesRemaining > 0)
            {
                Respawn();  
                StartCoroutine(InvulnerabilityPeriod());

                // Cancel power-ups on enemy collision:
                if (isTurboActive) 
                {
                    isTurboActive = false;
                    // You might want to  add a visual/audio effect here 
                }
                if (isTimeWarpActive) 
                {
                    isTimeWarpActive = false;
                    Time.timeScale = 1.0f; // Reset time scale

                    // Reset Enemy Speeds
                    foreach (EnemyCarMovement enemyMovement in enemyMovements) 
                    {
                        enemyMovement.ResetSpeedFromTimeWarp(); 
                    }

                    // Reset Power-Up Speeds
                    foreach (PowerUpMovement powerUpMovement in powerUpMovements)
                    {
                        powerUpMovement.ResetSpeedFromTimeWarp();
                    }
                }
            }
            else 
            {
               StartCoroutine(GameOverSequence()); 
            }
        }
    }

   void Respawn()
    {
        StartCoroutine(RespawnWithEffects());
    }

    IEnumerator RespawnWithEffects()
    {

         // 2. Play respawn effect (with slight delay if needed)
        if (respawnEffectPrefab != null)
        {
            GameObject respawnInstance = Instantiate(respawnEffectPrefab, respawnPosition, Quaternion.identity);

            // Adjust the delay if needed for timing with longer effects
            yield return new WaitForSeconds(0.3f);  

            Destroy(respawnInstance);
        }

        // 1. Respawn immediately at the respawn position
        transform.position = respawnPosition;

        // 3. Disable movement temporarily
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Reset any existing velocity
        GetComponent<Rigidbody2D>().isKinematic = true;  // Make the car non-reactive to physics

        yield return new WaitForSeconds(1f); // Wait for 1 second

        // 4. Re-enable player movement and physics interaction
        GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.SetActive(true); // Enable the car
    }


    IEnumerator InvulnerabilityPeriod()
    {
        // Make car temporarily invulnerable (e.g., by disabling its collider)
        GetComponent<Collider2D>().enabled = false; 

        yield return new WaitForSeconds(invulnerabilityDuration);

        // Re-enable the collider
        GetComponent<Collider2D>().enabled = true; 
    }

    void ActivateTurboBoost()
    {
        isTurboActive = true;
        originalSpeed = currentSpeed; // Store the speed before the boost 
        StartCoroutine(TurboBoostTimer());
    }

    IEnumerator TurboBoostTimer() 
    {
        yield return new WaitForSeconds(turboBoostDuration);
        isTurboActive = false; 
    }

    void ReplenishLife()
    {
        if (livesRemaining < maxLives) {
            livesRemaining++;
            Debug.Log("Life Replenished!");
            // You might want to add visual or sound effects here
        } 
    }

    void ActivateTimeWarp()
    {
        isTimeWarpActive = true;
        Time.timeScale = timeWarpSlowdownFactor;
        // Slowdown Track (Remember to adjust the TrackMovement script)

        // Slowdown Enemies
        foreach (EnemyCarMovement enemyMovement in enemyMovements) 
        {
            enemyMovement.UpdateSpeedForTimeWarp(timeWarpSlowdownFactor);
        }

        // Slowdown Power-ups
        foreach (PowerUpMovement powerUpMovement in powerUpMovements)
        {
            powerUpMovement.UpdateSpeedForTimeWarp(timeWarpSlowdownFactor);
        }

        StartCoroutine(TimeWarpTimer()); 
    }

    IEnumerator TimeWarpTimer() 
    {
        yield return new WaitForSeconds(timeWarpDuration);
        Time.timeScale = 1.0f;
        // Reset Speeds (Modify movement scripts here too)

        isTimeWarpActive = false; 
    }
    
    public GameObject jetFighterPrefab;
    private GameObject jetFighterInstance;
    private JetFighterMovement jetFighterMovementScript;
    public float jetFighterYOffset = 2.0f; 
    private float originalCarY; 
    public const float PERMANENT_Y_OFFSET = -3f; 

    IEnumerator ActivateJetFighter()
    {

        originalCarY = transform.position.y; // Store player car's y

        yield return new WaitForSeconds(0.01f); 

        gameObject.SetActive(false);

        // Calculate jet fighter position without directly using the car's position
        Vector3 jetFighterPosition = new Vector3(transform.position.x, originalCarY + jetFighterYOffset, transform.position.z);

        jetFighterInstance = Instantiate(jetFighterPrefab, jetFighterPosition, transform.rotation);
        jetFighterMovementScript = jetFighterInstance.GetComponent<JetFighterMovement>();
        jetFighterMovementScript.playerCarMovement = this; 

        jetFighterMovementScript.StartFighterTimer(); 
    }

    IEnumerator GameOverSequence() 
    {
        if (explosionEffectPrefab != null)
        {
            GameObject explosionInstance = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosionInstance, 0.5f);  // Adjust the delay  as needed
        }

        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<SpriteRenderer>().enabled = false; 

        yield return new WaitForSeconds(0.5f); // Wait for explosion to play 

        // Execute Game Over Logic
        if (gameOverMenu != null) 
        {
            gameOverMenu.ShowGameOver();
        }
        else 
        {
            Debug.LogError("GameOverMenu reference is missing!");
        }
    }


}