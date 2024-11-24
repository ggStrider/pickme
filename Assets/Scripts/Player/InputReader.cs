using UnityEngine;
using UnityEngine.InputSystem;

using Dialogue;

namespace Player
{
    [RequireComponent(typeof(PlayerSystem))]
    [RequireComponent(typeof(PlayerCameraRotate))]
    public class InputReader : MonoBehaviour
    {
        private PlayerMap _playerMap;
        
        private PlayerSystem _playerSystem;
        private PlayerCameraRotate _playerCameraRotate;
        
        private DialogueManager _dialogueManager;
        private PeeControl _peeControl;

        public void Initialize(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
            
            _playerSystem = GetComponent<PlayerSystem>();
            _playerCameraRotate = GetComponent<PlayerCameraRotate>();
            _peeControl = FindObjectOfType<PeeControl>();
            
            _playerMap = new PlayerMap();

            _playerMap.Main.Move.performed += OnMove;
            _playerMap.Main.Move.canceled += OnMove;
            
            _playerMap.Main.Sprint.started += OnSprint;
            _playerMap.Main.Sprint.canceled += OnSprint;

            _playerMap.Main.Interact.performed += OnInteract;
            _playerMap.Main.Interact.canceled += OnInteract;
            
            _playerMap.Main.Look.performed += OnLook;
            _playerMap.Main.Look.canceled += OnLook;
            
            // _playerMap.Main.Look.started += OnControlPee;
            // _playerMap.Main.Look.canceled += OnControlPee;
            
            _playerMap.Main.LMB.started += OnLeftMousePressed;
            
            _playerMap.Enable();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnControlPee(InputAction.CallbackContext context)
        {
            const float sensitivity = 0.01f;
            var y = context.ReadValue<Vector2>().x * sensitivity;
            _peeControl.SetDirection(y);
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            _playerSystem.Interact(context.ReadValue<float>() > 0);
        }

        private void OnLeftMousePressed(InputAction.CallbackContext context)
        {
            _dialogueManager.NextLine();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _playerSystem.SetDirection(direction);
        }
        
        private void OnSprint(InputAction.CallbackContext context)
        {
            _playerSystem.ToggleSprint(context.started);
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _playerCameraRotate.SetLookRotation(direction);
        }
    }
}