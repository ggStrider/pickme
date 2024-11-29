using UnityEngine;

using Dialogue;
using Dialogue.Observers;

using Handlers;
using Handlers.Observer;

namespace Player
{
    public class PlayerCameraRotate : MonoBehaviour, IDialogueStarted, IDialogueEnded, IFocused, IPlayerCameraSet, INewCameraSet
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private bool _canRotate = true;
        
        [Space]
        [SerializeField] private float _sensitivity = 0.3f;
        
        [Space]
        [SerializeField] private float _verticalMinAngle = -80f;
        [SerializeField] private float _verticalMaxAngle = 80f;
        
        private float _rotationX;
        private float _rotationY;

        private void Start()
        {
            if (_camera != null) return;
                
            Debug.LogWarning("Camera is null. Getting MainCamera...");

            if (Camera.main == null)
            {
                Debug.LogWarning("Main camera is null! Camera in component is null.");
                return;
            }
            
            _camera = Camera.main.transform;
        }

        public void Initialize(DialogueManager dialogueManager, PlayerCameraFocusHandler focusHandler, CamerasHandler camerasHandler)
        {
            dialogueManager.SubscribeDialogueStarted(this);
            dialogueManager.SubscribeDialogueEnded(this);
            
            camerasHandler.SubscribeToNewCameraSet(this);
            camerasHandler.SubscribeToPlayerCameraSet(this);
            
            focusHandler.Subscribe(this);
        }

        /// <summary>
        /// Method to rotate player and camera
        /// </summary>
        /// <param name="lookRotation">rotate vector</param>
        public void SetLookRotation(Vector2 lookRotation)
        {
            if (!_canRotate) return;
            var finalRotationVector = lookRotation * _sensitivity;
            
            _rotationX -= finalRotationVector.y;
            _rotationX = Mathf.Clamp(_rotationX, _verticalMinAngle, _verticalMaxAngle);

            _rotationY += finalRotationVector.x;
            _camera.localRotation = Quaternion.Euler(_rotationX, _rotationY, _camera.localRotation.z);
        }

        #region Observer stuff
        
        public void OnDialogueStarted(bool canControl)
        {
            _canRotate = canControl;
        }

        public void OnAllDialoguesEnded()
        {
            _canRotate = true;
        }

        public void OnCameraFocused(Vector3 newRotation)
        {
            _rotationX = newRotation.x;
            _rotationY = newRotation.y;
        }

        public void OnPlayerCameraSet()
        {
            _canRotate = true;
        }

        public void OnNewCameraSet()
        {
            _canRotate = false;
        }
        
        #endregion
    }
}