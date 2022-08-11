using DG.Tweening;
using Game.Scripts.Helpers.Extensions;
using UnityEngine;

namespace Game.Scripts.Resource
{
    public class GoldController : MonoBehaviour
    {
        [SerializeField] private float reachDuration;
        [SerializeField] private AnimationCurve movementYCurve;
        
        private PlayerController _playerController;
        private bool _isAlreadyFollowing;

        private void Start()
        {
            _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        private void StartFollowing()
        {
            _isAlreadyFollowing = true;

            DOVirtual.Float(0, 1f, reachDuration, position =>
            {
                transform.position = Vector3.Lerp(transform.position, _playerController.transform.position.WithY(.5f), position) 
                                     + new Vector3(0f, movementYCurve.Evaluate(position), 0f);
                
                transform.rotation = Quaternion.Lerp(transform.rotation, _playerController.transform.rotation, position);
            }).OnComplete(() =>
            {
                _playerController.CollectGold();
                Destroy(gameObject);
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (_isAlreadyFollowing) return;
            
            StartFollowing();
        }
    }
}
