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
        public bool grounded;
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
            Move();
        }

        private void Move()
        {
            if (Attacking) return;

            if (grounded) return;
            var targetPosition = _playerTransform.position;

            transform.SlowLookAt(targetPosition.WithY(transform.position.y), lookSpeed);

            enemyRigidbody.velocity = transform.forward * moveSpeed;
        }

        public void LookAt(Vector3 targetPosition, float delay = 0)
        {
            transform.SlowLookAt(targetPosition.WithY(transform.position.y), delay);
        }
        private void EnableMove()
        {
            animator.SetTrigger(movementAnimation);

        }

        private void DisableMove()
        {
            StopMovement();

        }

        private void OnDeath()
        {
            Dead = true;
            grounded = true;
            StopMovement();
            animator.SetTrigger("Death");
            deathAudio.Play();
            gameObject.GetComponent<Collider>().isTrigger = true;
            SpawnMonitor.Instance.RecordDead();
            mesh.material.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    if (isBoss)
                    {
                        BossHealth.Instance.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    }

                    var goldChance = UnityEngine.Random.Range(0, 100) <= 20;
                    if(goldChance)  {
                        var spawnedGold = Instantiate(gold);
                        spawnedGold.transform.position = transform.position;
                    }

                    StopMovement();
                    var c = Color.gray;
                    mesh.material.color = c;
                    Destroy(gameObject, 1f);
                });

        }

        void StopMovement()
        {
            enemyRigidbody.velocity = Vector3.zero;
            enemyRigidbody.angularVelocity = Vector3.zero;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Trap")) return;

            if (isBoss && !Attacking)
            {
                grounded = true;
                BossHealth.Instance.UpdateHealth(health);
                StopMovement();
                var collider = gameObject.GetComponent<Collider>();
                collider.enabled = false;
                animator.SetTrigger("Death");
                DOVirtual.DelayedCall(2f, () =>
                    {
                        grounded = false;
                        collider.enabled = true;
                        animator.SetTrigger(movementAnimation);
                    });

            }
            health--;
            if (health <= 0)
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
