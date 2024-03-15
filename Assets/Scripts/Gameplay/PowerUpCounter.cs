using UnityEngine;
using System.Collections; 
using TMPro; // Add this line

public class PowerUpCounter : MonoBehaviour
{
    // You'll need references to the TextMeshProUGUI elements if you want to manipulate them directly in this script
    public TextMeshProUGUI turboCounterText; 
    public TextMeshProUGUI timeWarpCounterText; 
    public TextMeshProUGUI jetFighterCounterText;  

    // References for power-up activation text
    public TextMeshProUGUI turboActivatedText;
    public TextMeshProUGUI timeWarpActivatedText;
    public TextMeshProUGUI jetFighterActivatedText;

    private int turboCount = 0;
    private int timeWarpCount = 0;
    private int jetFighterCount = 0;
    void Start()
    {
        // Initialize the counter displays on start
        UpdateTurboCounterDisplay();
        UpdateTimeWarpCounterDisplay();
        UpdateJetFighterCounterDisplay();
    }

    // Update counter functions
    public void IncrementTurboCounter()
    {
        turboCount++;
        UpdateTurboCounterDisplay();
    }

    public void IncrementTimeWarpCounter()
    {
        timeWarpCount++;
        UpdateTimeWarpCounterDisplay();
    }

    public void IncrementJetFighterCounter()
    {
        jetFighterCount++;
        UpdateJetFighterCounterDisplay();
    }

    public void ResetTurboCounter()
    {
        turboCount = 0;
        UpdateTurboCounterDisplay();
    }

    public void ResetTimeWarpCounter()
    {
        timeWarpCount = 0;
        UpdateTimeWarpCounterDisplay();
    }

    public void ResetJetFighterCounter()
    {
        jetFighterCount = 0;
        UpdateJetFighterCounterDisplay();
    }

    // Helper functions to update the TextMeshPro components 
    private void UpdateTurboCounterDisplay()
    {
        if (turboCounterText != null)
            turboCounterText.text = turboCount.ToString();
    }

    private void UpdateTimeWarpCounterDisplay()
    {
        if (timeWarpCounterText != null)
            timeWarpCounterText.text = timeWarpCount.ToString();
    }

    private void UpdateJetFighterCounterDisplay()
    {
        if (jetFighterCounterText != null)
            jetFighterCounterText.text = jetFighterCount.ToString();
    }

    public IEnumerator DisplayTurboActivatedText()
    {
        Debug.Log("Displaying Turbo Activated Text");
        turboActivatedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        turboActivatedText.gameObject.SetActive(false);
    }

    public IEnumerator DisplayTimeWarpActivatedText()
    {
        Debug.Log("Displaying TW Activated Text");
        timeWarpActivatedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        timeWarpActivatedText.gameObject.SetActive(false);
    }

    public IEnumerator DisplayJetFighterActivatedText()
    {
        Debug.Log("Displaying Jet Activated Text");
        jetFighterActivatedText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        jetFighterActivatedText.gameObject.SetActive(false);
    }

}
