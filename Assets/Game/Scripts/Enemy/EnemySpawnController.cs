using System.Collections;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Enemy
{
    public class EnemySpawnController : Singleton<EnemySpawnController>
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float power;
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private  EnemyController bossPrefab;
        [SerializeField] private float spawnCheckOffset;

        [SerializeField] private float minionSpawnInterval;
        [SerializeField] private float increaseMinionLimitInterval;

        [SerializeField] private GameObject heartPrefab;
        private int _currentSpawnRate;
        private int spawnId;

        private bool _shouldSpawn;

        public int aliveMinionsCount;
        private int _minionLimit = 1;
        
        EnemyController bossController;
        
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

        private void SpawnHeart()
        {
            if (!_shouldSpawn) return;
            
            var randomDirection = Random.insideUnitCircle.normalized;
            randomDirection *= Random.Range(4f, 5f);

            var spawnPoint = playerTransform.position;
            spawnPoint.x += randomDirection.x;
            spawnPoint.z += randomDirection.y;
            spawnPoint.y = 0f;
            
            var dist = Vector3.Distance(playerTransform.position, Vector3.zero);

            if(dist > 7) {
                spawnPoint = Random.insideUnitCircle.normalized * 5f;
                spawnPoint.y = 0f;
            } 

            var heart = Instantiate(heartPrefab);;
            heart.gameObject.transform.position = spawnPoint;
        }

        private void SpawnEnemy(bool boss=false)
        {
            if (!_shouldSpawn) return;

            aliveMinionsCount++;

            var spawnPoint = GetRandomPosition();
            var spawnedEnemyType = boss ? bossPrefab : enemyPrefab;
            
            var spawnedEnemy = Instantiate(spawnedEnemyType);
            if(boss) {
                bossController = spawnedEnemy;
            }
            
            spawnedEnemy.gameObject.transform.position = spawnPoint;
            SpawnMonitor.Instance.RecordSpawn();
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
