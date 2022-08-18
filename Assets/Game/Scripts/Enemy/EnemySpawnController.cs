using System.Collections;
using Game.Scripts.Helpers.Pooling;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Enemy
{
    public class EnemySpawnController : Singleton<EnemySpawnController>
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float power;
        [SerializeField] private float spawnCheckOffset;

        [SerializeField] private float minionSpawnInterval;
        [SerializeField] private float increaseMinionLimitInterval;

        private int _spawnCap = 250;
        private int _currentSpawnRate;
        private int _spawnId;

        private bool _shouldSpawn;

        public int aliveMinionsCount;
        private int _minionLimit = 1;

        public void StartSpawning()
        {
            _shouldSpawn = true;
            
            StartCoroutine(IncreaseMinionLimit());
            StartCoroutine(SpawnMinion());
        }

        public void StopSpawning()
        {
            _shouldSpawn = false;
        }

        private IEnumerator IncreaseMinionLimit()
        {
            while (true)
            {
                yield return new WaitForSeconds(increaseMinionLimitInterval);
                if (_minionLimit >= _spawnCap) break;
                
                _minionLimit++;
            }
        }

        private IEnumerator SpawnMinion()
        {
            while (_shouldSpawn)
            {
                yield return new WaitUntil(() => aliveMinionsCount < _minionLimit);
                SpawnEnemy();

                yield return new WaitForSeconds(minionSpawnInterval);
            }
        }

        private void SpawnEnemy()
        {
            if (!_shouldSpawn) return;

            aliveMinionsCount++;

            var spawnPoint = GetRandomPosition();
            var spawnedEnemy = PoolManager.Instance.GetObject<EnemyController>();
            spawnedEnemy.gameObject.transform.position = spawnPoint;
        }

        private Vector3 GetRandomPosition()
        {
            while (true)
            {
                var spawnPoint = playerTransform.position;
                var randomDirection = Random.insideUnitCircle.normalized * power;

                spawnPoint.x += randomDirection.x;
                spawnPoint.z += randomDirection.y;
                spawnPoint.y = 0f;
                    
                if (spawnPoint.x < -20 + spawnCheckOffset || spawnPoint.x > 20 - spawnCheckOffset) continue;
                if (spawnPoint.z < -20 + spawnCheckOffset || spawnPoint.z > 20 - spawnCheckOffset) continue;

                return spawnPoint;
            }
        }
    }
}
