using Cinemachine;
using UnityEngine;

using Data;

using Dialogue;
using Dialogue.Observers;

using GetObjects;
using Handlers;
using Handlers.Observer;
using Hover;
using Interact;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(HoveredCheckRay))]
    [RequireComponent(typeof(PlayerCameraFocusHandler))]
    [RequireComponent(typeof(CameraTilt))]
    [RequireComponent(typeof(SprintSystem))]
    public class PlayerSystem : MonoBehaviour, IDialogueStarted, IDialogueEnded, IPlayerCameraSet, INewCameraSet
    {
        [SerializeField] private float _maxSpeed = 10f;
        [SerializeField] private float _addSpeedDelta = 1f;
        [SerializeField] private float _currentSpeed;
        [SerializeField] private bool _canMove = true;

        [Space]
        [SerializeField] private float _interactionDistance = 3f;
        
        [Space]
        [SerializeField] private Transform _playerCamera;

        private CharacterController _characterController;

        private IInteractProp _currentInHand;
        
        private Vector2 _direction;

        public delegate void IsMoving(Vector2 direction);
        public event IsMoving OnIsMoving;

        public void Initialize(DialogueManager dialogueManager, CamerasHandler camerasHandler)
        {
            _characterController = GetComponent<CharacterController>();

            GetComponent<HoveredCheckRay>().SetSettings(_playerCamera, _interactionDistance);
            
            dialogueManager.SubscribeDialogueStarted(this);
            dialogueManager.SubscribeDialogueEnded(this);
            
            camerasHandler.SubscribeToPlayerCameraSet(this);
            camerasHandler.SubscribeToNewCameraSet(this);

            var virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            GetComponent<PlayerCameraFocusHandler>().GetCamera(virtualCamera);

            GetComponent<CameraTilt>()?.Initialize(_playerCamera, this);

            var sprint = GetComponent<SprintSystem>();
            sprint.OnSprintToggled += OnSprintToggled;
        }

        private void OnSprintToggled(bool isSprinting, float speedBoost)
        {
            _maxSpeed += speedBoost;
        }
        
        /// <summary>
        /// Sets movement direction for player 
        /// </summary>
        /// <param name="direction">Movement vector</param>
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
            
            OnIsMoving?.Invoke(_canMove ? direction : Vector2.zero);
            
            // If player is not moving, set current speed to 0
            if(_direction != Vector2.zero) return;
            _currentSpeed = 0;
        }

        private void FixedUpdate()
        {
            if(_canMove) Move();
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

        public void Interact(bool isPressing)
        {
            var objectInRay = GetObjectByRay.Get(_playerCamera.position,
                _playerCamera.forward, _interactionDistance, DefaultData.TriggerLayer);

            objectInRay?.GetComponent<IInteract>()?.Interact(isPressing);
        }

        public void InteractWithProp(bool isPressing)
        {
            if (_currentInHand != null && !isPressing)
            {
                _currentInHand.Interact(false);

                _currentInHand = null;
                return;
            }
            
            var prop = GetObjectByRay.Get(_playerCamera.position,
                _playerCamera.forward, _interactionDistance, DefaultData.TriggerLayer)?
                .GetComponent<IInteractProp>();
            
            if(prop is null) return;
            
            _currentInHand = prop;
            prop.Interact(isPressing);
        }

        private void OnDisable()
        {
            var sprint = GetComponent<SprintSystem>();
            sprint.OnSprintToggled -= OnSprintToggled;
        }
        
        public void OnDialogueStarted(bool canControl)
        {
            _canMove = canControl;
        }

        public void OnAllDialoguesEnded()
        {
            _canMove = true;
        }

        public void OnPlayerCameraSet()
        {
            _canMove = true;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnNewCameraSet()
        {
            _canMove = false;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
