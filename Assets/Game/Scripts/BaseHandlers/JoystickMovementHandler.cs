using UnityEngine;

namespace Game.Scripts.BaseHandlers
{
    [RequireComponent(typeof(Rigidbody))]
    public class JoystickMovementHandler : MonoBehaviour
    {
        [SerializeField] private float actorMovementSpeed;
        [SerializeField] private float actorRotateSpeed;
        
        private Rigidbody _actorRigidbody;

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
            _actorRigidbody.velocity = transform.forward * actorMovementSpeed;
        }

        public void Stop()
        {
            _actorRigidbody.velocity = Vector3.zero;
        }
    }
}
