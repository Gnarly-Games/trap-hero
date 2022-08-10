using UnityEngine;

namespace Game.Scripts.Helpers.Pooling
{
    public abstract class PoolObject : MonoBehaviour
    {
        public abstract void OnDeactivate();
        public abstract void OnSpawn();
        public abstract void OnCreated();
    }
}
