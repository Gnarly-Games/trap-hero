using UnityEngine;

namespace Game.Scripts
{
    public class DebugConsoleHandler : MonoBehaviour
    {
        [SerializeField] private GameObject debugConsole;
        private int _clickCount;

        public void IncreaseClickCount()
        {
            _clickCount++;
            if (_clickCount < 10) return;
            
            debugConsole.SetActive(true);
        }
    }
}
