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
        gameObject.SetActive(true);
        _health = (float)health;
    }

    // Update is called once per frame
    public void UpdateHealth(int value)
    {
        if (value <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            healthBar.value = value / _health;

        }
    }
}
