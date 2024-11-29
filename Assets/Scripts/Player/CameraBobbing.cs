using UnityEngine;

namespace Player
{
    public class CameraBobbing : MonoBehaviour
    {
        [SerializeField] private bool _enableBobbing = true;
        
        [SerializeField] private Transform _cameraParent;
        
        [SerializeField, Range(0f, 15f)] private float _bobbingFrequency = 5f;
        [SerializeField, Range(0f, 1f)] private float _bobbingAmplitude = 0.05f;
        private float _bobbingPhase;

        [Space]
        [SerializeField] private float _boostFrequencyOnSprint = 2f;
        private bool _isPlayerMoving;
        
        private float _bobbingValue;

        private void Start()
        {
            var sprintSystem = GetComponent<SprintSystem>();
            sprintSystem.OnSprintToggled += HandleBobbingOnSprint;
         
            var playerSystem = GetComponent<PlayerSystem>();
            playerSystem.OnIsMoving += IsPlayerMoving;
        }

        private void Update()
        {
            if(!_isPlayerMoving || !_enableBobbing) return;

            // Using sinusoid to smoothly change camera position
            _bobbingValue += Time.deltaTime;
            var sinusValue = Mathf.Sin(_bobbingValue * _bobbingFrequency) * _bobbingAmplitude;

            _cameraParent.localPosition = Vector3.up * sinusValue;
        }

        private void IsPlayerMoving(Vector2 direction)
        {
            _isPlayerMoving = direction != Vector2.zero;
        }

        private void HandleBobbingOnSprint(bool isSprinting, float value)
        {
            _bobbingPhase = _bobbingValue * _bobbingFrequency;
            
            if (isSprinting)
            {
                _bobbingFrequency += _boostFrequencyOnSprint;
            }
            else
            {
                _bobbingFrequency -= _boostFrequencyOnSprint;
            }
            
            _bobbingValue = _bobbingPhase / _bobbingFrequency;
        }

        private void OnDisable()
        {
            var sprintSystem = GetComponent<SprintSystem>();
            sprintSystem.OnSprintToggled -= HandleBobbingOnSprint;
         
            var playerSystem = GetComponent<PlayerSystem>();
            playerSystem.OnIsMoving -= IsPlayerMoving;
        }
    }
}
