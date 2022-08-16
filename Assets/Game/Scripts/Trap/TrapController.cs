using System.Collections;
using UnityEngine;

namespace Game.Scripts.Trap
{
    public class TrapController : MonoBehaviour
    {
        [SerializeField] private GameObject trap;
        
        [SerializeField] private float activeDuration;
        [SerializeField] private float chargeDuration;

        private bool _isTrapActive;

        private IEnumerator ActivateTrap()
        {
            _isTrapActive = true;
            yield return new WaitForSeconds(chargeDuration);
            trap.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(activeDuration);
            trap.gameObject.SetActive(false);
            _isTrapActive = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_isTrapActive) return;
            
            StartCoroutine(ActivateTrap());
        }
    }
}
