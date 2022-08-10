using System.Collections.Generic;
using MyBox;
using UnityEngine;

namespace Game.Scripts.Helpers.Pooling
{
    public class PoolManager : Singleton<PoolManager>
    {
        [System.Serializable]
        internal struct Pool
        {
            [MustBeAssigned]
            [SerializeField] internal PoolObject objectPrefab;
            
            [PositiveValueOnly]
            [SerializeField] internal int poolSize;
            
            internal Queue<PoolObject> PooledObjects;
        }

        [SerializeField]
        private Pool[] pools;
        
        private void Start()
        {
            CreatePools();
        }

        private void CreatePools()
        {
            for (var i = 0; i < pools.Length; i++)
            {
                pools[i].PooledObjects = new Queue<PoolObject>();

                for (var j = 0; j < pools[i].poolSize; j++)
                {
                    var poolObject = Instantiate(pools[i].objectPrefab, transform);

                    pools[i].PooledObjects.Enqueue(poolObject);

                    poolObject.OnCreated();
                }

            }
        }

        public T GetObject<T>() where T : PoolObject
        {
            for (var i = 0; i < pools.Length; i++)
            {
                if (typeof(T) != pools[i].objectPrefab.GetType()) continue;
                
                var poolObject = pools[i].PooledObjects.Dequeue() as T;
                pools[i].PooledObjects.Enqueue(poolObject);
                if (poolObject == null) continue;
                
                poolObject.OnSpawn();
                return poolObject;
            }
            return default;
        }

    }
}
