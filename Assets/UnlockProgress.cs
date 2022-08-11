using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockProgress : MonoBehaviour
{
    [SerializeField] private TMP_Text progressText;
    
    [SerializeField] private Slider progressBar;
    public float Total = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateValue(float current)
    {
        var cost = (Total - current);
        progressBar.value =  cost / Total;
        progressText.text = $"{(int)(cost)}/{(int)Total}";
    }
}
