using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Game.Scripts.Enemy;
using UnityEngine.UI;

public interface IAttackType
{
    public MonoBehaviour MonoBehaviour { get; }
}

public class Boss : MonoBehaviour
{
    public MonoBehaviour MonoBehaviour => this;
    public float attackRange;
    public float attackInterval;
    public GameObject jumpIndicatorPrefab;
    public float nextAttackTime;
    private Rigidbody _rigidbody;
    private bool _isAttackState;
    private Vector3 _attackDirection;
    private float _hitEffectEndTime;
    private EnemyController _monster;
    private GameObject _target;
    public BossHealth healthBar;

    private void Awake()
    {
        _monster = GetComponent<EnemyController>();

        _rigidbody = GetComponent<Rigidbody>();
        _target = GameObject.FindGameObjectWithTag("Player");
        _monster.movementAnimation = "Walking";
        _monster.health = 5;
        healthBar = GameObject.FindObjectOfType<BossHealth>();
        healthBar.gameObject.SetActive(true);
        healthBar.SetHealth(_monster.health);

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            Destroy(other.gameObject);
            if(transform.localScale.x <=3) {
                transform.localScale *= 1.05f;
            }
        };
  
    }  

    public void DashAttack()
    {
        var distance = Vector3.Distance(_target.transform.position, transform.position);
        if (distance <= attackRange)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackInterval;
                _monster.Attacking = true;
                var monsterPosition = _monster.transform.position;
                monsterPosition.y = 0;
                _attackDirection = (_target.transform.position - monsterPosition).normalized;
                _attackDirection.y = 0;
                jumpIndicatorPrefab.SetActive(true);
                jumpIndicatorPrefab.transform.position = monsterPosition;
                jumpIndicatorPrefab.transform.forward = _attackDirection;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _monster.LookAt(_target.transform.position);
                _monster.animator.SetTrigger("DynIdle");
                transform.DOShakeScale(1f, 0.1f, 1).OnComplete(() =>
                {
                    
                    _isAttackState = true;
                    var target = jumpIndicatorPrefab.transform.position + _attackDirection * 8;
                    target += Vector3.up;
                            jumpIndicatorPrefab.SetActive(false);

                    _monster.animator.SetTrigger("Spin");

                    _rigidbody.DOMove(target, 0.7f)
                        .OnComplete(() =>
                        {
                            _isAttackState = false;
                            _monster.animator.SetTrigger(_monster.movementAnimation);
                            _monster.Attacking = false;
                        });
                });
            }
        }


        if (_monster.Attacking)
        {
            if (jumpIndicatorPrefab)
            {
                jumpIndicatorPrefab.transform.forward = _attackDirection;
            }
        }
    }
    private void Update()
    {
        
        DashAttack();

    }

}
