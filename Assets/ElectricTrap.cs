using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrap : MonoBehaviour
{
     // Start is called before the first frame update
    public AudioSource audio;

    public static ElectricTrap Instance;

    public void Awake() {
        Instance = this;
    }
    void Start()
    {
    }
   
    public void Play() {
        audio.Play();
    }
    
}
