using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Main
{
    public class InputReader : MonoBehaviour
    {
        public event Action<Vector2> OnMove;
        public event Action<bool> OnLeftMousePressStateChanged;
        public event Action<bool> OnSprintStateChanged;
        public event Action<Vector2> OnMouseLook;
        public event Action<bool> OnInteractButtonPressStateChanged;
        
        private PlayerMap _playerMap;
        
        private void Awake()
        {
            _playerMap = new PlayerMap();

            _playerMap.Main.Move.performed += context => NotifyVector2(context, OnMove);
            _playerMap.Main.Move.canceled += context => NotifyVector2(context, OnMove);

            _playerMap.Main.Sprint.started += _ => OnSprintStateChanged?.Invoke(true);
            _playerMap.Main.Sprint.canceled += _ => OnSprintStateChanged?.Invoke(false);

            _playerMap.Main.Interact.performed += _ => OnInteractButtonPressStateChanged?.Invoke(true);
            _playerMap.Main.Interact.canceled += _ => OnInteractButtonPressStateChanged?.Invoke(false);

            _playerMap.Main.Look.performed += context => NotifyVector2(context, OnMouseLook);
            _playerMap.Main.Look.canceled += context => NotifyVector2(context, OnMouseLook);

            _playerMap.Main.LMB.started += _ => OnLeftMousePressStateChanged?.Invoke(true);
            _playerMap.Main.LMB.canceled += _ => OnLeftMousePressStateChanged?.Invoke(false);
            
            _playerMap.Main.Enable();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDestroy()
        {
            _playerMap?.Dispose();

            OnMove = null;
            OnLeftMousePressStateChanged = null;
            OnSprintStateChanged = null;
            OnMouseLook = null;
            OnInteractButtonPressStateChanged = null;
        }

        private void NotifyVector2(InputAction.CallbackContext context, Action<Vector2> action)
        {
            var direction = context.ReadValue<Vector2>();
            action?.Invoke(direction);
        }
    }
}