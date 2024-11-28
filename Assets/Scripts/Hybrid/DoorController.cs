using UnityEngine;

using Interact;
using UnityEngine.InputSystem;

namespace Hybrid
{
    public class DoorController : MonoBehaviour, IInteractProp
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private float _sensitivity = 0.2f;

        [SerializeField] private float _minAngle;
        [SerializeField] private float _maxAngle;

        [SerializeField] private Transform _player;
        [SerializeField] private Transform _side1;
        [SerializeField] private Transform _side2;
        
        [SerializeField] private bool _inverseMultiplier;
        
        private sbyte _sideMultiplier;
        
        private float _currentRotation;

        private void Start()
        {
            if (_pivot == null)
            {
                _pivot = transform.parent ?? transform;
            }
            
            if (transform.parent == null)
            {
                Debug.Log("No pivot parent on game object. Pivot now is transform");
            }

            if (_minAngle > _maxAngle)
            {
                (_minAngle, _maxAngle) = (_maxAngle, _minAngle);
            }
            
            _currentRotation = _pivot.rotation.eulerAngles.y;
        }

        private void Update()
        {
            if ((_player.position - _side1.position).magnitude > (_player.position - _side2.position).magnitude)
            {
                _sideMultiplier = _inverseMultiplier ? (sbyte)-1 : (sbyte)1;
            }
            else
            {
                _sideMultiplier = _inverseMultiplier ? (sbyte)1 : (sbyte)-1;
            }
            
            var mouseX = Mouse.current.delta.ReadValue().x * (_sensitivity * _sideMultiplier);
            
            _currentRotation = Mathf.Clamp(_currentRotation + mouseX, _minAngle, _maxAngle);
            _pivot.rotation = Quaternion.Euler(0, _currentRotation, 0);
        }
        
        public void Interact(bool isPressing)
        {
            enabled = isPressing;
        }
        
        #if UNITY_EDITOR

        public void _SetClosePosition()
        {
            _SetPivot();
            _minAngle = _pivot.rotation.eulerAngles.y;
        }

        public void _SetOpenPosition()
        {
            _SetPivot();
            _maxAngle = _pivot.rotation.eulerAngles.y;
        }

        public void _SetPivot()
        {
            if (_pivot == null)
            {
                _pivot = transform.parent ?? transform;
            }
        }
        #endif
    }
}
