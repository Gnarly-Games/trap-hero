using System;
using DG.Tweening;
using Game.Scripts.Enemy;
using UnityEngine;

public class SphereTrap : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rigidbody;
    public AudioSource audio;

    [SerializeField] private Color activeColor;
    [SerializeField] private Color originalColor;
    [SerializeField] private Renderer trapRenderer;

    private bool _isTrapActive;
    
    void Start()
    {
        audio.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody.velocity.magnitude >= 1)
        {
            if (!_isTrapActive)
            {
                _isTrapActive = true;
                trapRenderer.material.DOColor(activeColor, 0.25f);
            }
            
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            audio.volume = rigidbody.velocity.magnitude / 30f;

            //Play sound here if you have a rigidbody component and if your movement is rigidbody.AddForce
        }
        else
        {
            if (_isTrapActive)
            {
                _isTrapActive = false;
                trapRenderer.material.DOColor(originalColor, 0.25f);
            }
            
            if (audio.isPlaying)
            {
                audio.Pause();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.velocity.magnitude < 1) return;
        if (!collision.gameObject.CompareTag("Enemy")) return;
        
        collision.gameObject.GetComponent<EnemyController>().GetDamage();  
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (rigidbody.velocity.magnitude < 1) return;
        if (!collisionInfo.gameObject.CompareTag("Enemy")) return;
        
        collisionInfo.gameObject.GetComponent<EnemyController>().GetDamage(); 
    }
}
