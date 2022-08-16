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
        [SerializeField] private  EnemyController bossPrefab;

        [SerializeField] private GameObject heartPrefab;
        private int _currentSpawnRate;
        private int spawnId;
        
        EnemyController bossController;
        private void Start()
        {
            _currentSpawnRate = 1;
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
                if(_currentSpawnRate < spawnCount) {
                    _currentSpawnRate += 1;
                    SpawnMonitor.Instance.UpdateSpawnRate(_currentSpawnRate);
                }
                spawnId++;
                if(spawnId % 7 == 0) {
                    SpawnHeart();
                }
                if(spawnId % 1 == 0 && bossController==null) {
                    SpawnEnemy(true);
                }
            }
        }
        private void SpawnHeart()
        {
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
            var spawnPoint = Vector3.zero;
            var dist = Vector3.Distance(playerTransform.position, Vector3.zero);
            var randomDirection = Random.insideUnitCircle.normalized;

            if(dist > 8) {
                randomDirection *= Random.Range(4f, 6f);
            }  else {
                spawnPoint = playerTransform.position;
                randomDirection *= Random.Range(12f, 15f);
            }
            spawnPoint.x += randomDirection.x;
            spawnPoint.z += randomDirection.y;
            spawnPoint.y = 0f;
            var spawnedEnemyType = boss ? bossPrefab : enemyPrefab;
            
            var spawnedEnemy = Instantiate(spawnedEnemyType);
            if(boss) {
                bossController = spawnedEnemy;
            }
            spawnedEnemy.gameObject.transform.position = spawnPoint;
            SpawnMonitor.Instance.RecordSpawn();
        }
    }
}
