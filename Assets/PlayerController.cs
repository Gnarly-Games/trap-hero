using System;
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
    [SerializeField] private Slider healthBarBackground;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Color regenColor;
    [SerializeField] private CanvasGroup healthBarCanvas;
    [SerializeField] private GameObject joyStick;
    [SerializeField] private Image hitPanel;
    [SerializeField] private AudioSource coinAudio;
    [SerializeField] private AudioSource healthAudio;
    
    private Color _originalBodyColor;
    private Color _originalHealthColor;
    private bool _isDead;

    public int goldAmount;

    private void Start()
    {
        GameManager.Instance.onLevelFailed.AddListener(() => playerCollider.enabled = false);

        _originalHealthColor = healthBarFill.color;
        healthBar.value = health / 100f;
        healthBarCanvas.alpha = 0f;
        
        _originalBodyColor = meshRenderer.material.color;
    }

    public void GetDamage()
    {
        healthBarCanvas.DOFade(1, 0.25f);
        if (_isDead) return;
        
        health -= enemyDamage;
        
        healthBar.value = health / 100f;
        DOVirtual.DelayedCall(0.2f, () => healthBarBackground.DOValue(healthBar.value, 0.25f));
        
        hitPanel.gameObject.SetActive(true);
        meshRenderer.material.DOColor(Color.white, 0.1f)
            .SetLoops(3)
            .OnComplete(() => {
                meshRenderer.material.DOColor(_originalBodyColor, 0.1f);
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
        if (other.CompareTag("Enemy")) 
        {
            var controller = other.gameObject.GetComponent<EnemyController>();
            if(controller.dead) return;
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
            healthBarFill.DOColor(regenColor, 0.2f)
                .OnComplete(() => healthBarFill.DOColor(_originalHealthColor, 0.2f));
            
            health = Math.Min(100, health+10);
            healthAudio.Play();
            healthBar.value = health / 100f;
            healthBarBackground.value = healthBar.value;
            
            other.gameObject.SetActive(false);
            
            if (health == 100) healthBarCanvas.DOFade(0, 0.25f);
        }
    }
}
