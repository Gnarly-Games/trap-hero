using DG.Tweening;
using Game.Scripts.Core;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    public int health;
    
    [SerializeField] private Collider playerCollider;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject joyStick;
    
    [SerializeField] private VoidEvent onDamageReceived;
    [SerializeField] private IntEvent onGoldAmountChanged;

    private Color _originalColor;

    public int goldAmount;

    private void Start()
    {
        GameManager.Instance.onLevelFailed.AddListener(() => playerCollider.enabled = false);
        _originalColor = meshRenderer.material.color;
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        
        onDamageReceived.Raise();
        meshRenderer.material.DOColor(Color.white, 0.1f)
            .SetLoops(3)
            .OnComplete(() => {
                meshRenderer.material.DOColor(_originalColor, 0.1f);
            });
        
        if (health <= 0) OnDeath();
    }

    private void OnDeath()
    {
        playerAnimator.SetBool("Running", false);
        playerAnimator.SetBool("Death", true);
        
        joyStick.SetActive(false);
        
        GameManager.Instance.onLevelFailed?.Invoke();
    }

    private void CollectGold()
    {
        goldAmount++;
        onGoldAmountChanged.Raise(goldAmount);
    }

    public void RemoveGold()
    {
        if (goldAmount == 0) return;
        
        goldAmount--;
        onGoldAmountChanged.Raise(goldAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Gold")) return;
        
        CollectGold();
        Destroy(other.gameObject);
    }
}
