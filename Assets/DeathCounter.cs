using UnityEngine;
using TMPro; // Include this for TextMeshPro support

public class DeathCounter : MonoBehaviour
{
    public TextMeshProUGUI deathText; // Assign the TextMeshPro UI element in the Inspector
    private int deathCount = 0;       // Tracks the number of deaths

    // Call this method when the player dies
    public void IncrementDeathCount()
    {
        deathCount++; // Increment the count
        UpdateDeathText(); // Update the UI
    }

    // Updates the text on the UI
    private void UpdateDeathText()
    {
        if (deathText != null)
        {
            deathText.text = "Deaths: " + deathCount;
        }
        else
        {
            Debug.LogError("deathText is not assigned in the Inspector!");
        }
    }
}
