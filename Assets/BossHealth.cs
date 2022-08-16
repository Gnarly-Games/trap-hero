using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public static BossHealth Instance;
    private Slider healthBar;
    private float _health;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        healthBar = GetComponent<Slider>();
        
    }
    public void SetHealth(int health)
    {
        GetComponent<CanvasGroup>().alpha = 1;
        _health = (float)health;
        UpdateHealth(health);
    }

    // Update is called once per frame
    public void UpdateHealth(int value)
    {
        if (value <= 0)
        {
            GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            healthBar.value = value / _health;

        }
    }
}
