using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Trap
{
    public class TrapController : MonoBehaviour
    {
        [SerializeField] private GameObject trap;
        
        [SerializeField] private float enableInterval;
        [SerializeField] private float activeDuration;
        public UnityEvent onActivate;
        private void Start()
        {
            StartCoroutine(ActivateTrap());
        }

        private IEnumerator ActivateTrap()
        {
            while (true)
            {
                yield return new WaitForSeconds(enableInterval);
            
                trap.gameObject.SetActive(true);
                onActivate?.Invoke();
                yield return new WaitForSeconds(activeDuration);
            
                trap.gameObject.SetActive(false);
            }
        }
    }
}
