using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timer;
    public string timerFormatted;
    public bool running;
    public TMP_Text timerText;
    public static Timer Instance;

    private float _passedTime;
    
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timer = 1;
    }

    public void ResetTimer()
    {
        timer = 1;
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
        System.TimeSpan t = TimeSpan.FromSeconds(timer);
        timerFormatted = string.Format("{0:D2}m {1:D2}s", t.Minutes, t.Seconds);
        timerText.text = $"Time Elapsed: {timerFormatted}";
    }
    void Update()
    {
        if (running)
        {
            _passedTime += Time.deltaTime;
            if (_passedTime >= 1f)
            {
                UpdateText();
                timer += 1;
                SpawnMonitor.Instance.IncreaseScore(1);
                _passedTime = 0f;
            }
        }
        
        

    }
}
