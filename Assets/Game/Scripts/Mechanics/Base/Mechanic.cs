using UnityEngine;

namespace Game.Scripts.Mechanics.Base
{
    public abstract class Mechanic : MonoBehaviour
    {
        public abstract void OnDown();
        public abstract void OnDrag();
        public abstract void OnUp();
        public abstract void OnMechanicEnabled();
        public abstract void OnMechanicDisabled();
    }
}
