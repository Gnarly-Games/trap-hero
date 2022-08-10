using Game.Scripts.Mechanics.Base;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Core
{
    public class MechanicManager : Singleton<MechanicManager>
    {
        [SerializeField] private Mechanic activeMechanic;
        
        private bool _isDragging;

        private UnityAction _onUpdate;
        private UnityAction _onDown;
        private UnityAction _onDrag;
        private UnityAction _onUp;
        
        // Gets invoked when game initialized. Listener for "onGameInitialized" event.
        public void Initialize()
        {
            if (activeMechanic == null) return;
            
            _onDown = activeMechanic.OnDown;
            _onDrag = activeMechanic.OnDrag;
            _onUp = activeMechanic.OnUp;
        }

        private void Update()
        {
            _onUpdate?.Invoke();
        }

        private void HandleTouch()
        {
            if (!_isDragging)
            {
                if (!Input.GetMouseButtonDown(0)) return;
                
                _onDown?.Invoke();
                _isDragging = true;
            }
            else
            {
                _onDrag?.Invoke();

                if (!Input.GetMouseButtonUp(0)) return;
                
                _onUp?.Invoke();
                _isDragging = false;
            }
        }

        // Gets invoked when level is started. Listener for "onLevelStarted" event.
        public void EnableInput()
        {
            if (activeMechanic == null) return;
            
            _onUpdate += HandleTouch;
            activeMechanic.OnMechanicEnabled();
        }
        
        // Gets invoked when level is ended. Listener for "onLevelEnded" event.
        public void DisableInput()
        {
            if (activeMechanic == null) return;
            
            _onUpdate -= HandleTouch;
            activeMechanic.OnMechanicDisabled();
        }
    }
}
