using UnityEngine;
using Zenject;

using System;

using Dialogue.Observers;
using Dialogue;

using Creature;

using Handlers;
using Handlers.Observer;

using Player.Additional;
using _Configs.Player.Movement;

namespace Player.Main
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IMovable, IDialogueStarted, IDialogueEnded,
        IPlayerCameraSet, INewCameraSet
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private float _currentSpeed;
        [SerializeField] private bool _canMove = true;
        private float _maxSpeed;

        [Space]
        [SerializeField] private Transform _playerCamera;

        private CharacterController _characterController;

        private Vector2 _direction;

        public event Action<Vector2> OnIsMoving;

        private InputReader _inputReader;
        private SprintSystem _sprintSystem;

        [Inject]
        private void Construct(DialogueManager dialogueManager, CamerasHandler camerasHandler, 
            SprintSystem sprintSystem, InputReader inputReader)
        {
            dialogueManager.SubscribeDialogueStarted(this);
            dialogueManager.SubscribeDialogueEnded(this);
            
            camerasHandler.SubscribeToPlayerCameraSet(this);
            camerasHandler.SubscribeToNewCameraSet(this);

            sprintSystem.OnSprintToggled += OnSprintToggled;
            inputReader.OnMove += SetDirection;
        }

        private void OnDestroy()
        {
            if(_inputReader != null) _inputReader.OnMove -= SetDirection;
            if(_sprintSystem != null) _sprintSystem.OnSprintToggled -= OnSprintToggled;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            if (_playerConfig == null)
            {
                throw new NullReferenceException("No player config on PlayerMovement!!!");
            }
            
            ChangePlayerConfig(_playerConfig);
        }
        
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
            
            OnIsMoving?.Invoke(_canMove ? direction : Vector2.zero);
            
            if(_direction != Vector2.zero) return;
            _currentSpeed = 0;
        }

        private void FixedUpdate()
        {
            if(_canMove) Move();
        }

        public void Move()
        {
            if (IsPlayerMoving())
            {
                _currentSpeed = Mathf.Clamp(_currentSpeed + _playerConfig.SpeedAcceleration, 0, _maxSpeed);
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

        private bool IsPlayerMoving()
        {
            if (!_canMove) return false;
            return _direction != Vector2.zero;
        }

        public void ChangePlayerConfig(PlayerConfig playerConfig)
        {
            _playerConfig = playerConfig;
            _maxSpeed = playerConfig.MaxSpeed;
        }

        #region Subscriptions
        
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
        
        private void OnSprintToggled(bool isSprinting, float speedBoost)
        {
            _maxSpeed += speedBoost;
        }
        
        #endregion
    }
}
