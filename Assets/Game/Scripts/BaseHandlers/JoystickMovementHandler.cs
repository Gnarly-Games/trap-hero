using UnityEngine;

namespace Game.Scripts.BaseHandlers
{
    [RequireComponent(typeof(Rigidbody))]
    public class JoystickMovementHandler : MonoBehaviour
    {
        [SerializeField] private float actorMovementSpeed;
        [SerializeField] private float actorRotateSpeed;
        [SerializeField] private Animator animator;
        private Rigidbody _actorRigidbody;
        
        private bool _moving;
        private void Start()
        {
            _actorRigidbody = GetComponent<Rigidbody>();
        }
        
        public void Rotate(Quaternion newRotation)
        {
            _actorRigidbody.rotation = Quaternion.Lerp(_actorRigidbody.rotation, newRotation, actorRotateSpeed * Time.deltaTime);
        }

        public void Move()
        {
            if(!_moving) {
                _moving = true;
                animator.SetBool("Running", _moving);
                animator.SetBool("DynIdle", !_moving);
            }
            _actorRigidbody.velocity = transform.forward * actorMovementSpeed;
        }

        public void Stop()
        {
            if(_moving) {
                _moving = false;
                animator.SetBool("Running", _moving);
                animator.SetBool("DynIdle", !_moving);
            }
            _actorRigidbody.velocity = Vector3.zero;
        }
    }
}
