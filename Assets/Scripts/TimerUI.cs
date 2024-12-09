using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign in the Inspector
    private float elapsedTime = 0f; // Time counter

    void Update()
    {
        
            elapsedTime += Time.deltaTime;

            // Calculate minutes, seconds, and milliseconds
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

            // Format the timer as MM:SS:MS
            timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
        }
    
}
