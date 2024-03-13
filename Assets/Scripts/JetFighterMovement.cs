using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetFighterMovement : MonoBehaviour
{
    public float jetFighterSpeed = 10f; // Notice: Changed the name for clarity
    public float minX = -4.17f; 
    public float maxX = 4.24f;
    public float jetFighterDuration = 10f; 
    
    private Vector3 position;
    public PlayerCarMovement playerCarMovement;

    void Start() 
    {
        position = transform.position;
    }

    void Update()
    {
        position.x += Input.GetAxis("Horizontal") * jetFighterSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        transform.position = position;
    }

    // Start the timer for the jet fighter
    public void StartFighterTimer()
    {
    StartCoroutine(JetFighterTimer());
    }

    IEnumerator JetFighterTimer() 
    {
        yield return new WaitForSeconds(jetFighterDuration);

        if (playerCarMovement != null) 
        {
            playerCarMovement.gameObject.SetActive(true);

            // Calculate correct position. We'll use the jet fighter's current X-position
            Vector3 carPosition = new Vector3(transform.position.x, transform.position.y + PlayerCarMovement.PERMANENT_Y_OFFSET + playerCarMovement.jetFighterYOffset, transform.position.z); 
            // Force Y-position to -3.85
            playerCarMovement.transform.position = new Vector3(carPosition.x, -3.85f, carPosition.z);
        }   
        Destroy(gameObject); 
    }
    


}