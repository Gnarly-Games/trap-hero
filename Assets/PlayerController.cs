using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Core;
using Game.Scripts.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int enemyDamage;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject joyStick;
    [SerializeField] private Image hitPanel;
    [SerializeField] private AudioSource coinAudio;
    [SerializeField] private AudioSource healthAudio;

    private Color _originalColor;
    private bool _isDead;

    public int goldAmount;

    private void Start()
    {
        GameManager.Instance.onLevelFailed.AddListener(() => playerCollider.enabled = false);
        healthBar.value = health / 100f;
        _originalColor = meshRenderer.material.color;
    }

    public void GetDamage()
    {
        if (_isDead) return;
        
        health -= enemyDamage;
        healthBar.value = health / 100f;
        hitPanel.gameObject.SetActive(true);
        meshRenderer.material.DOColor(Color.white, 0.1f)
            .SetLoops(3)
            .OnComplete(() => {
                meshRenderer.material.DOColor(_originalColor, 0.1f);
                hitPanel.gameObject.SetActive(false);
            });
        
        if (health > 0) return;
        _isDead = true;
        playerAnimator.SetTrigger("Death");
        joyStick.SetActive(false);
        
        GameManager.Instance.onLevelFailed?.Invoke();
        GameManager.Instance.isGameRunning = false;
    }

    private void CollectGold()
    {
        goldAmount++;
        goldText.text = goldAmount.ToString();
    }

    public void RemoveGold()
    {
        if (goldAmount == 0) return;
        
        goldAmount--;
        goldText.text = goldAmount.ToString();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) {
            var controller = other.gameObject.GetComponent<EnemyController>();
            if(controller.Dead) return;
            GetDamage();
        }
        
        if (other.CompareTag("Gold"))
        {
            CollectGold();
            coinAudio.Play();
            Destroy(other.gameObject);
        }
        
        if (other.CompareTag("Heart"))
        {
            health = Math.Min(100, health+10);
            healthAudio.Play();
            healthBar.value = health / 100f;
            Destroy(other.gameObject);
        }
    }
}
