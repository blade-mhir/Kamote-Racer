using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class LivesRemainingV : MonoBehaviour
{
    public Image[] livesImages; // Assign the array of images in the Inspector
    public PlayerCarMovement playerCar; // Assign this in the Inspector

    void Update()
    {
        if (playerCar != null)
        {
            int lives = playerCar.GetLivesRemaining(); 

            // Enable/Disable images based on lives
            for (int i = 0; i < livesImages.Length; i++)
            {
                livesImages[i].enabled = (i < lives);
            }
        }
    }
}
