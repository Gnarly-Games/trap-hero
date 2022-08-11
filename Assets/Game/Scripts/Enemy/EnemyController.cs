using System;
using DG.Tweening;
using Game.Scripts.Core;
using Game.Scripts.Helpers.Extensions;
using Game.Scripts.Helpers.Pooling;
using UnityEngine;

namespace Game.Scripts.Enemy
{
    public class EnemyController : PoolObject
    {
        [SerializeField] private GameObject gold;
        [SerializeField] private float lookSpeed;
        [SerializeField] private float moveSpeed;
        [SerializeField] private SkinnedMeshRenderer mesh;
        [SerializeField] private Rigidbody enemyRigidbody;
        [SerializeField] private Animator animator;
        
        private Transform _playerTransform;

        private Action _updatePool;
        private bool _moving;
        private float _nextDirectionChange;
        public bool Dead;
        private void Start()
        {
            _playerTransform = GameObject.FindWithTag("Player").transform;

            GameManager.Instance.onLevelStarted.AddListener(EnableMove);
            GameManager.Instance.onLevelFailed.AddListener(DisableMove);
            
            if (GameManager.Instance.isGameRunning) EnableMove();
        }
        
        private void Update()
        {
            _updatePool?.Invoke();
        }

        private void Move()
        {
            if(!_moving) {
                _moving = true;
                animator.SetBool("Running", true);
            }
            var targetPosition = _playerTransform.position;

            if(_nextDirectionChange < Time.time ) {
                _nextDirectionChange = Time.time + 0.02f;
            }
            transform.SlowLookAt(targetPosition.WithY(transform.position.y), lookSpeed);

            enemyRigidbody.velocity = transform.forward * moveSpeed;
        }

        private void EnableMove()
        {
            _updatePool += Move;
        }

        private void DisableMove()
        {
            _updatePool -= Move;
            enemyRigidbody.velocity = Vector3.zero;
             
        }

        private void OnDeath()
        {
            Dead = true;
            DisableMove();
            animator.SetBool("Running", false);
            animator.SetBool("Death", true);

            mesh.material.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {

                    _moving = false;
                    var spawnedGold = Instantiate(gold);
                    spawnedGold.transform.position = transform.position;
                    enemyRigidbody.velocity = Vector3.zero;
                    enemyRigidbody.angularVelocity = Vector3.zero;
                    var c = Color.black;
                    c.a = 0.4f;
                    mesh.material.color = c;
                    Destroy(gameObject, 1f);
                });

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Trap")) return;
            
            OnDeath();
        }

        #region Pool

        public override void OnDeactivate()
        {
            gameObject.SetActive(false);
        }

        public override void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public override void OnCreated()
        {
            OnDeactivate();
        }

        #endregion
    }
}
