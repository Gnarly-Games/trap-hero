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
        [SerializeField] private GameObject heartPrefab;
        private int _currentSpawnRate;
        private int spawnId;
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
        private void SpawnEnemy()
        {
            var randomDirection = Random.insideUnitCircle.normalized;
            randomDirection *= Random.Range(power, power * 1.5f);
            
            var spawnPoint = playerTransform.position;
            spawnPoint.x += randomDirection.x;
            spawnPoint.z += randomDirection.y;
            spawnPoint.y = 0f;

            var dist = Vector3.Distance(playerTransform.position, Vector3.zero);

            if(dist > 7) {
                spawnPoint = Random.insideUnitCircle.normalized * 5f;
                spawnPoint.y = 0f;
            } 
            

            var spawnedEnemy = Instantiate(enemyPrefab);;
            spawnedEnemy.gameObject.transform.position = spawnPoint;
            SpawnMonitor.Instance.RecordSpawn();
        }
    }
}
