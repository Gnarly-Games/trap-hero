using Game.Scripts.BaseHandlers;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Mechanics.Base
{
    public class JoystickMovementMechanic : Mechanic
    {
        [Separator("Visuals")]
        [SerializeField] private bool shouldShowJoystickVisuals;
        
        [ConditionalField(nameof(shouldShowJoystickVisuals))]
        [SerializeField] private Image joystickBackgroundImage, joystickHandleImage;

        [Separator("Logic")] 
        [SerializeField] private JoystickMovementHandler actorToControl;

        private Vector3 _firstInputPosition;
        private Vector3 _lastInputPosition;
        private Vector3 _inputPositionDifference;
        
        private float _joystickOuterSize;

        private void Start()
        {
            _joystickOuterSize = joystickBackgroundImage.rectTransform.rect.width / 2f - joystickHandleImage.rectTransform.rect.width / 2f;
        }

        public override void OnDown()
        {
            _firstInputPosition = Input.mousePosition;
            
            EnableJoystickVisual();
        }

        public override void OnDrag()
        {
            _lastInputPosition = Input.mousePosition;
            if (_firstInputPosition == _lastInputPosition) return;
            
            _inputPositionDifference = _lastInputPosition - _firstInputPosition;
            
            MoveActor();
            SetImagePositions();
        }
        
        public override void OnUp()
        {
            actorToControl.Stop();
            DisableJoystickVisual();
        }
        
        public override void OnMechanicEnabled()
        {
            EnableJoystickVisual();
        }

        public override void OnMechanicDisabled()
        {
            OnUp();
        }

        private void MoveActor()
        {
            actorToControl.Rotate(CalculateNewRotation());
            actorToControl.Move();
        }
        
        private Quaternion CalculateNewRotation()
        {
            var normalizedDifference = _inputPositionDifference.normalized;
            var newRotation = Mathf.Atan2(normalizedDifference.y, normalizedDifference.x) * Mathf.Rad2Deg - 90f;

            return Quaternion.Euler(Vector3.down * newRotation);
        }

        private void EnableJoystickVisual()
        {
            if (shouldShowJoystickVisuals == false) return;
            
            joystickBackgroundImage.transform.position = _firstInputPosition;
            joystickBackgroundImage.gameObject.SetActive(true);
        }

        private void DisableJoystickVisual()
        {
            if (shouldShowJoystickVisuals == false) return;
            
            joystickBackgroundImage.gameObject.SetActive(false);
            joystickHandleImage.transform.localPosition = Vector3.zero;
        }

        private void SetImagePositions()
        {
            if (shouldShowJoystickVisuals == false) return;
            
            if (_inputPositionDifference.magnitude >= _joystickOuterSize)
            {
                joystickHandleImage.transform.localPosition = _inputPositionDifference.normalized * _joystickOuterSize;
            }
            else
            {
                joystickHandleImage.transform.localPosition = _inputPositionDifference;
            }
        }
    }
}
