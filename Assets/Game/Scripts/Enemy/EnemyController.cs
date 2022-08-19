using System;
using System.Collections;
using DG.Tweening;
using Game.Scripts.Core;
using Game.Scripts.Helpers.Extensions;
using Game.Scripts.Helpers.Pooling;
using Game.Scripts.Score;
using Game.Scripts.UI;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Game.Scripts.Enemy
{
    public class EnemyController : PoolObject
    {
        [SerializeField] private float lookSpeed;
        [SerializeField] private float moveSpeed;
        [SerializeField] private SkinnedMeshRenderer mesh;
        [SerializeField] private Rigidbody enemyRigidbody;
        [SerializeField] private float attackInterval;
        [SerializeField] private int score;

        [SerializeField] private AudioSource deathAudio;

        private PlayerController _playerController;

        private Action _updatePool;
        
        private float _nextDirectionChange;
        public bool dead;
        public Animator animator;

        public bool attacking;
        public string movementAnimation = "Running";
        public int health;
        public bool isBoss;
        public bool grounded;
        private bool _shouldAttack;
        
        private void Start()
        {
            _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            GameManager.Instance.onLevelStarted.AddListener(EnableMove);
            GameManager.Instance.onLevelCompleted.AddListener(DisableMove);

            if (GameManager.Instance.isGameRunning) EnableMove();

            isBoss = gameObject.GetComponent<Boss>() != null;
            
            StartCoroutine(StartAttack());
        }

        private void Update()
        {
            _updatePool?.Invoke();
        }

        private void Move()
        {
            if (attacking) return;
            if (grounded) return;
            
            var targetPosition = _playerController.transform.position;

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
            _updatePool += Move;
        }

        private void DisableMove()
        {
            StopMovement();
            _updatePool -= Move;
            animator.SetTrigger("DynIdle");
        }

        public void GetDamage()
        {
            if (isBoss && !attacking)
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
            if (health <= 0 && !dead)
                OnDeath();
        }

        private void OnDeath()
        {
            ScoreHandler.Instance.IncreaseScore(score);
            ScoreHandler.Instance.ShowScore(transform.position.WithY(1.5f), score);
            EnemySpawnController.Instance.aliveMinionsCount--;
            
            dead = true;
            grounded = true;
            StopMovement();
            animator.SetTrigger("Death");
            deathAudio.Play();
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            gameObject.GetComponent<Collider>().isTrigger = true;
            mesh.material.DOColor(Color.white, 0.1f).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    if (isBoss)
                    {
                        BossHealth.Instance.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                    }

                    var heartChance = UnityEngine.Random.Range(0, 100) <= 20;
                    if(heartChance)
                    {
                        var heart = PoolManager.Instance.GetObject<HeartController>();
                        heart.transform.position = transform.position;
                    }

                    StopMovement();
                    var c = Color.gray;
                    mesh.material.color = c;

                    DOVirtual.DelayedCall(1f, () =>
                    {
                        gameObject.SetActive(false);
                        Reset();
                    });
                });

        }

        private void Reset()
        {
            animator.SetTrigger("DynIdle");
            mesh.material.color = Color.white;
        }

        private IEnumerator StartAttack()
        {
            while (!dead)
            {
                yield return new WaitUntil(() => _shouldAttack);
                
                if (dead) continue;
                _playerController.GetDamage();
                
                yield return new WaitForSeconds(attackInterval);
            }
        }

        void StopMovement()
        {
            enemyRigidbody.velocity = Vector3.zero;
            enemyRigidbody.angularVelocity = Vector3.zero;
        }
        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                _shouldAttack = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) _shouldAttack = false;
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
