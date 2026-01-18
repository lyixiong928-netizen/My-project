using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{
    public Text timeText;
    private float elapsedTime = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            DisplayTime();
        }
    }

    void DisplayTime()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 100f) % 100f);
            timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    public void PauseStart()
    {
        isRunning = !isRunning;
    }

    public void Reset()
    {
        elapsedTime = 0f;
        isRunning = false;
        DisplayTime();
    }
}
