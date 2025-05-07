using UnityEngine;
using TMPro; // Use TMP namespace

public class Timer : MonoBehaviour
{
    public TMP_Text timerText; // Reference to a TextMeshPro Text component
    private float timeElapsed;

    void Update()
    {
        timeElapsed += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
