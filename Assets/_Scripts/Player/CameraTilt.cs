using UnityEngine;
using Zenject;

using Creature;

namespace Player
{
    public class CameraTilt : MonoBehaviour
    {
        [SerializeField] private Transform _cameraToTilt;
        
        [SerializeField] private float _currentTiltAngle;
        [SerializeField] private float _maxTiltAngle = 1.5f;
        
        [SerializeField] private float _changeTiltAngleDelta = 0.005f;
        [SerializeField] private float _resetTiltAngleDelta = 0.005f;
        [SerializeField] private float _addWhenGoAnotherSide = 0.005f;
        
        private const float MAX_TILT_ERROR = 0.01f;
        
        private Vector3 _direction;

        private IMovable _movable;
        
        [Inject]
        private void Construct(IMovable movable)
        {
            _movable = movable;
            movable.OnIsMoving += GetDirection;
        }

        private void Awake()
        {
            if (!_cameraToTilt)
            {
                throw new MissingComponentException("Missing camera to tilt");
            }
        }

        private void OnDestroy()
        {
            _movable.OnIsMoving -= GetDirection;
        }
        
        private void LateUpdate()
        {
            if (!_cameraToTilt) return;
            
            // Right
            if (_direction.x > 0)
            {
                var delta = _changeTiltAngleDelta;
                if (_currentTiltAngle < 0)
                {
                    delta += _addWhenGoAnotherSide;
                }
                _currentTiltAngle = Mathf.Clamp(_currentTiltAngle - delta,
                    -_maxTiltAngle, _maxTiltAngle);
            }
            
            // Left
            else if (_direction.x < 0)
            {
                var delta = _changeTiltAngleDelta;
                if (_currentTiltAngle > 0)
                {
                    delta += _addWhenGoAnotherSide;
                }
                
                _currentTiltAngle = Mathf.Clamp(_currentTiltAngle + delta,
                    -_maxTiltAngle, _maxTiltAngle);
            }
            
            // Forward or Stop
            else
            {
                _currentTiltAngle = Mathf.Lerp(_currentTiltAngle, 0, _resetTiltAngleDelta);
                if (Mathf.Abs(_currentTiltAngle) <= MAX_TILT_ERROR) _currentTiltAngle = 0;
            }
            
            var rotationVector = new Vector3(_cameraToTilt.eulerAngles.x,
                _cameraToTilt.eulerAngles.y, _currentTiltAngle);
            _cameraToTilt.localRotation = Quaternion.Euler(rotationVector);
        }

        private void GetDirection(Vector2 direction)
        {
            _direction = direction;
        }
    }
}
