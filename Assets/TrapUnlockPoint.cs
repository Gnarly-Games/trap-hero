using System.Collections;
using UnityEngine;

public class TrapUnlockPoint : MonoBehaviour
{
    [SerializeField] private int unlockPrice;
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private float payInterval;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UnlockProgress progress;
    [SerializeField] private AudioSource unlockAudio;
    [SerializeField] private AudioSource transferAudio;

    private bool _isPlayerInside;

    private void Start()
    {
        progress.Total = unlockPrice;
        progress.UpdateValue(unlockPrice);
        StartCoroutine(PaymentProcess());
    }

    private IEnumerator PaymentProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(payInterval);
            if (!_isPlayerInside) continue;
            
            if(playerController.goldAmount > 0) {
                playerController.RemoveGold();
                Pay();
            }
        }
    }

    private void Pay()
    {
        unlockPrice--;
        progress.UpdateValue((float)unlockPrice);
        if(unlockPrice%3 == 0) 
            transferAudio.Play();

        if (unlockPrice > 0) return;
        unlockAudio.Play();

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
