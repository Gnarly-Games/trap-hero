using System;
using DG.Tweening;
using Game.Scripts.Core;
using Game.Scripts.Helpers.Extensions;
using Game.Scripts.Helpers.Pooling;
using UnityAtoms.BaseAtoms;
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
        [SerializeField] private int damage;
        
        private PlayerController _playerController;

        private Action _updatePool;
        private bool _isDead;

        private void Start()
        {
            _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

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
            var targetPosition = _playerController.transform.position;
            
            transform.SlowLookAt(targetPosition.WithY(transform.position.y), lookSpeed);
            enemyRigidbody.velocity = transform.forward * moveSpeed;
        }

        private void EnableMove()
        {
            _updatePool += Move;
            animator.SetBool("Running", true);
        }

        private void DisableMove()
        {
            _updatePool -= Move;
            enemyRigidbody.velocity = Vector3.zero;
        }

        private void DealDamage()
        {
            _playerController.GetDamage(damage);   
        }

        private void OnDeath()
        {
            _isDead = true;
            DisableMove();
            animator.SetBool("Running", false);
            animator.SetBool("Death", true);

            mesh.material.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
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
            if (other.CompareTag("Trap")) OnDeath();
            
            if (other.CompareTag("Player"))
            {
                if (!_isDead) DealDamage();
            }
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
