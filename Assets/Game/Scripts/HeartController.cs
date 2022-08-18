using Game.Scripts.Helpers.Pooling;
using UnityEngine;

namespace Game.Scripts
{
    public class HeartController : PoolObject
    {
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
    }
}
