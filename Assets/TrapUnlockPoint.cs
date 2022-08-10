using System.Collections;
using UnityEngine;

public class TrapUnlockPoint : MonoBehaviour
{
    [SerializeField] private int unlockPrice;
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float payInterval;
    [SerializeField] private PlayerController playerController;

    private bool _isPlayerInside;

    private void Start()
    {
        StartCoroutine(PaymentProcess());
    }

    private IEnumerator PaymentProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(payInterval);
            if (!_isPlayerInside) continue;
            
            playerController.RemoveGold();
            Pay();
        }
    }

    private void Pay()
    {
        unlockPrice--;
        if (unlockPrice > 0) return;

        var trap = Instantiate(trapPrefab);
        trap.transform.position = transform.position;
        
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _isPlayerInside = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _isPlayerInside = false;
    }
}
