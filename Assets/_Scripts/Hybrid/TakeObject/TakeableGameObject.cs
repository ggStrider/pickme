using Interact;
using UnityEngine;

namespace Hybrid.TakeObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class TakeableGameObject : MonoBehaviour, IInteractProp
    {
        [SerializeField] private float _smoothPosition = 3;
        [SerializeField] private float _smoothRotation = 1;
        
        private Rigidbody _rigidbody;
        private Transform _playerItemPlace;
        
        private const float DRAG_ON_TAKE = 15;
        private const float DRAG_ON_THROW = 1;
        private const float MAX_DISTANCE_TO_ATTACH = 3.3f;

        public void Initialize(Transform playerItemPlace)
        {
            enabled = false;
            _rigidbody = GetComponent<Rigidbody>();
            _playerItemPlace = playerItemPlace;

            if (playerItemPlace == null)
            {
                Debug.LogWarning("Player item place is null!!!");
            }
        }
        
        private void FixedUpdate()
        {
            if (!_playerItemPlace) return;

            var direction = _playerItemPlace.position - transform.position;
            _rigidbody.AddForce(direction * _smoothPosition, ForceMode.VelocityChange);

            var torqueAngle = Quaternion.Lerp(
                transform.rotation, _playerItemPlace.rotation, _smoothRotation);
            
            _rigidbody.MoveRotation(torqueAngle);

            if ((_playerItemPlace.position - transform.position).magnitude > MAX_DISTANCE_TO_ATTACH)
            {
                Interact(false);
            }
        }

        public void Interact(bool isPressing)
        {
            if (isPressing)
            {
                _rigidbody.useGravity = false;
                _rigidbody.drag = DRAG_ON_TAKE;
                
                enabled = true;
            }
            else
            {
                _rigidbody.useGravity = true;
                _rigidbody.drag = DRAG_ON_THROW;
                
                enabled = false;
            }
        }
    }
}
