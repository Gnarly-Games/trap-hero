using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.UI;
using TMPro;
using UnityEngine;

public class SpawnMonitor : MonoBehaviour
{
    // Start is called before the first frame update
    public int Total;
    public int Dead;
    
    public TMP_Text totalText;
    public TMP_Text spawnRateText;
    public TMP_Text deadCountText;
    public TMP_Text aliveCountText;
    
 public static SpawnMonitor Instance;
        void Awake() {
            Instance = this;
        }
        
    internal void RecordDead()
    {
        Dead++;
        deadCountText.text = "Dead: " + Dead.ToString();
    }

    internal void RecordSpawn()
    {
        Total++;
        aliveCountText.text = "Alive: " + (Total - Dead).ToString();
        totalText.text = "Total: " + Total.ToString();
    }

    internal void UpdateSpawnRate(int currentSpawnRate)
    {
        spawnRateText.text = "Rate: " + currentSpawnRate.ToString();
    }
}
