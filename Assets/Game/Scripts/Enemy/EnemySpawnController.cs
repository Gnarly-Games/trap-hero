using System.Collections;
using Game.Scripts.Helpers.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Enemy
{
    public class EnemySpawnController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float interval;
        [SerializeField] private int spawnCount;
        [SerializeField] private float power;
        [SerializeField] private EnemyController enemyPrefab;
        private int _currentSpawnRate;
        private void Start()
        {
            _currentSpawnRate = 5;
            for (var i = 0; i < _currentSpawnRate; i++)
            {
                SpawnEnemy();
            }

            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);

                var amount = Mathf.Min(_currentSpawnRate, spawnCount);
                for (var i = 0; i < amount; i++)
                {
                    SpawnEnemy();
                }
                
                _currentSpawnRate += 1;
            }
        }

        private void SpawnEnemy()
        {
            var randomDirection = Random.insideUnitCircle.normalized;
            randomDirection *= Random.Range(power, power * 1.5f);

            var spawnPoint = playerTransform.position;
            spawnPoint.x += randomDirection.x;
            spawnPoint.z += randomDirection.y;
            spawnPoint.y = 0f;

            var spawnedEnemy = Instantiate(enemyPrefab);
            spawnedEnemy.gameObject.transform.position = spawnPoint;
        }
    }
}
