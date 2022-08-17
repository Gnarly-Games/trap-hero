using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Trap
{
    public class TrapController : MonoBehaviour
    {
        [SerializeField] private GameObject trap;
        [SerializeField] private Transform planeTransform;
        
        [SerializeField] private float activeDuration;
        [SerializeField] private float chargeDuration;

        private bool _isTrapActive;
        private bool _isReadyToActivate;

        private void Start()
        {
            planeTransform.localScale = Vector3.zero;
        }

        private IEnumerator ActivateTrap()
        {
            _isTrapActive = true;
            
            StartCoroutine(PlaySnappedChargeAnimation());
            
            yield return new WaitUntil(() => _isReadyToActivate);
            trap.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(activeDuration);
            trap.gameObject.SetActive(false);

            _isReadyToActivate = false;
            _isTrapActive = false;
        }

        private IEnumerator PlaySnappedChargeAnimation()
        {
            var isCompleted = false;
            var snapValue = 1 / chargeDuration;
            
            var targetScale = planeTransform.localScale + snapValue * Vector3.one;
            planeTransform.DOScale(targetScale, 0.25f);

            while (!isCompleted)
            {
                yield return new WaitForSeconds(0.75f);
                
                targetScale = planeTransform.localScale + snapValue * Vector3.one;
                planeTransform.DOScale(targetScale, 0.25f)
                    .OnComplete(() =>
                    {
                        if (planeTransform.localScale.x >= 0.95f) isCompleted = true;
                    });
                
                yield return new WaitForSeconds(0.25f);
            }

            planeTransform.localScale = Vector3.zero;
            _isReadyToActivate = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_isTrapActive) return;
            
            StartCoroutine(ActivateTrap());
        }
    }
}
