using Cinemachine;
using Data;
using UnityEngine;

using Dialogue;
using Dialogue.Observers;
using GetObjects;
using Handlers;
using Hover;
using Interact;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(HoveredCheckRay))]
    [RequireComponent(typeof(CameraFocusHandler))]
    public class PlayerSystem : MonoBehaviour, IDialogueStarted, IDialogueEnded
    {
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _addSpeedDelta = 1f;
        [SerializeField] private float _currentSpeed;
        [SerializeField] private bool _canMove = true;
        private float _defaultMaxSpeed;

        [Space, Header("Sprint")]
        [SerializeField] private bool _isSprinting;
        [SerializeField] private float _speedBoostOnSprint = 5f;
        [SerializeField] private float _cantRunStamina = 3f;
        
        [SerializeField] private bool _canSprint = true;

        [Space, Header("Stamina")]
        [SerializeField] private bool _infiniteStamina;
        [SerializeField] private float _maxStamina = 100f;
        [SerializeField] private float _currentStamina;

        [SerializeField] private float _subtractStaminaDelta = 1;
        [SerializeField] private float _addStaminaDelta = 0.5f;
        
        [Space]
        [SerializeField] private float _interactionDistance = 3f;
        
        [SerializeField] private Transform _playerCamera;

        private CharacterController _characterController;
        
        private Vector2 _direction;

        public delegate void SprintAction(bool isSprinting);
        public event SprintAction OnSprintToggled;
        
        public void Initialize(DialogueManager dialogueManager)
        {
            _characterController = GetComponent<CharacterController>();

            _defaultMaxSpeed = _maxSpeed;
            _currentStamina = _maxStamina;

            if (_infiniteStamina)
            {
                _currentStamina = _cantRunStamina + 1;
            }
            
            GetComponent<HoveredCheckRay>().SetSettings(_playerCamera, _interactionDistance);
            
            dialogueManager.SubscribeDialogueStarted(this);
            dialogueManager.SubscribeDialogueEnded(this);

            var virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            GetComponent<CameraFocusHandler>().GetCamera(virtualCamera);
        }
        
        /// <summary>
        /// Sets movement direction for player 
        /// </summary>
        /// <param name="direction">Movement vector</param>
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
            
            // If player is not moving, set current speed to 0
            if(_direction != Vector2.zero) return;
            _currentSpeed = 0;
        }

        private void FixedUpdate()
        {
            if(_canMove) Move();
            UpdateStamina();
        }

        private void Move()
        {
            if (IsPlayerMoving())
            {
                _currentSpeed = Mathf.Clamp(_currentSpeed + _addSpeedDelta, 0, _maxSpeed);
            }
            
            var cameraForward = _playerCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            // direction.x in current context means right/left
            // direction.y in current context means forward/backwards
            var movementDirection = cameraForward * _direction.y + 
                                    _playerCamera.transform.right * _direction.x;
            movementDirection.Normalize();

            var finalMoveVector = movementDirection * Time.fixedDeltaTime;
            
            _characterController.SimpleMove(finalMoveVector.normalized * _currentSpeed);   
        }

        public bool IsPlayerMoving()
        {
            if (!_canMove) return false;
            return _direction != Vector2.zero;
        }

        private bool WasSprinting()
        {
            return _maxSpeed > _defaultMaxSpeed;
        }
        
        public void ToggleSprint(bool isPressed)
        {
            if(!_canSprint || _currentStamina <= _cantRunStamina) return;
            
            if (isPressed)
            {
                EnableSprint();
            }
            else
            {
                DisableSprint();
            }
        }

        private void EnableSprint()
        {
            _isSprinting = true;
            OnSprintToggled?.Invoke(_isSprinting);

            _maxSpeed += _speedBoostOnSprint;
            _currentSpeed += _speedBoostOnSprint;
        }

        private void DisableSprint()
        {
            _isSprinting = false;
            
            if(!WasSprinting()) return;
            OnSprintToggled?.Invoke(_isSprinting);

            _currentSpeed -= _speedBoostOnSprint;
            _maxSpeed -= _speedBoostOnSprint;
        }

        private void UpdateStamina()
        {
            if(_infiniteStamina) return;
            
            if (_isSprinting)
            {
                _currentStamina = Mathf.Clamp(_currentStamina - _subtractStaminaDelta, 0, _maxStamina);

                if (_currentStamina <= _cantRunStamina)
                {
                    DisableSprint();
                }
            }
            else
            {
                _currentStamina = Mathf.Clamp(_currentStamina + _addStaminaDelta, 0, _maxStamina);
            }
        }
        
        public void Interact(bool isPressing)
        {
            var objectInRay = GetObjectByRay.Get(_playerCamera.position,
                _playerCamera.forward, _interactionDistance, DefaultData.TriggerLayer);
            
            objectInRay?.GetComponent<IInteract>()?.Interact(isPressing);
        }

        public void OnDialogueStarted(bool canControl)
        {
            _canMove = canControl;
            
            // i should think bout that
            _canSprint = canControl;
        }

        public void OnAllDialoguesEnded()
        {
            _canMove = true;
            
            // i should think bout that
            _canSprint = true;
        }
    }
}
