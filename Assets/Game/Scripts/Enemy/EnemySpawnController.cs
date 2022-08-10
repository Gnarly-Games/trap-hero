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

        private void Start()
        {
            for (var i = 0; i < spawnCount; i++)
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

                for (var i = 0; i < spawnCount; i++)
                {
                    SpawnEnemy();
                }
            }
        }

        private void SpawnEnemy()
        {
            var randomDirection = Random.onUnitSphere;
            randomDirection *= power;

            var spawnPoint = playerTransform.position;
            spawnPoint.x += randomDirection.x;
            spawnPoint.z += randomDirection.z;
            spawnPoint.y = 0f;

            var spawnedEnemy = PoolManager.Instance.GetObject<EnemyController>();
            spawnedEnemy.transform.position = spawnPoint;
        }
    }
}
