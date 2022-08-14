using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timer;
    public string timerFormatted;
    public bool running;
    public TMP_Text timerText;
    public static Timer Instance;
    void Awake()
    {
        Instance = this;
    }
    public void ResetTimer()
    {
        timer = 0;
        UpdateText();
    }
    public void StartTimer()
    {
        ResetTimer();
        running = true;
    }
    public void StopTimer()
    {
        running = false;
    }

    void UpdateText()
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        timerFormatted = string.Format("{0:D2}m {1:D2}s", t.Minutes, t.Seconds);
        timerText.text = $"Time Elapsed: {timerFormatted}";
    }
    void Update()
    {
        if (running)
        {

            UpdateText();
            timer += Time.deltaTime;
        }

    }
}
