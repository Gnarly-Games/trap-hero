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
        public Animator animator;
        [SerializeField] private AudioSource deathAudio;
        
        private Transform _playerTransform;

        private Action _updatePool;
        private float _nextDirectionChange;
        public bool Dead;

        public bool Attacking;
        public string movementAnimation = "Running";
        public int health;
        public bool isBoss;
        private void Start()
        {
            _playerTransform = GameObject.FindWithTag("Player").transform;

            GameManager.Instance.onLevelStarted.AddListener(EnableMove);
            GameManager.Instance.onLevelFailed.AddListener(DisableMove);
            
            if (GameManager.Instance.isGameRunning) EnableMove();

            isBoss = gameObject.GetComponent<Boss>() != null;
        }
        
        private void Update()
        {
            _updatePool?.Invoke();
        }

        private void Move()
        {
            if(Attacking) return;
            var targetPosition = _playerTransform.position;

            transform.SlowLookAt(targetPosition.WithY(transform.position.y), lookSpeed);

            enemyRigidbody.velocity = transform.forward * moveSpeed;
        }

        public void LookAt(Vector3 targetPosition, float delay=0) {
            transform.SlowLookAt(targetPosition.WithY(transform.position.y), delay);
        }
        private void EnableMove()
        {
                animator.SetBool(movementAnimation, true);

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
            animator.SetBool(movementAnimation, false);
            animator.SetBool("Death", true);
            deathAudio.Play();
            gameObject.GetComponent<Collider>().isTrigger = true;
            SpawnMonitor.Instance.RecordDead();
            mesh.material.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {

                    var spawnedGold = Instantiate(gold);
                    spawnedGold.transform.position = transform.position;
                    enemyRigidbody.velocity = Vector3.zero;
                    enemyRigidbody.angularVelocity = Vector3.zero;
                    var c = Color.black;
                    c.a = 0.5f;
                    mesh.material.color = c;
                    Destroy(gameObject, 1f);
                });

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Trap")) return;
            health--;
            if(isBoss) {
                BossHealth.Instance.UpdateHealth(health);
            }
            if(health <= 0) 
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
