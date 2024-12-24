using UnityEngine;

using Dialogue;
using Dialogue.Observers;

using Handlers;
using Handlers.Observer;
using Zenject;

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

        private InputReader _inputReader;
        
        [Inject]
        private void Construct(InputReader inputReader, DialogueManager dialogueManager,
            PlayerCameraFocusHandler focusHandler, CamerasHandler camerasHandler)
        {
            _inputReader = inputReader;
            _inputReader.OnMouseLook += SetLookRotation;
            
            dialogueManager.SubscribeDialogueStarted(this);
            dialogueManager.SubscribeDialogueEnded(this);
            
            camerasHandler.SubscribeToNewCameraSet(this);
            camerasHandler.SubscribeToPlayerCameraSet(this);
            
            focusHandler.Subscribe(this);
        }

        private void OnDestroy()
        {
            if(_inputReader != null) _inputReader.OnMouseLook -= SetLookRotation;
        }
        
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
        /// <summary>
        /// Rotates camera by input
        /// </summary>
        /// <param name="lookRotation">rotate vector</param>
        private void SetLookRotation(Vector2 lookRotation)
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
            // Normalizing angle to prevent abrupt next rotation 
            var normalizedX = Mathf.DeltaAngle(0, newRotation.x);
            _rotationX = Mathf.Clamp(normalizedX, _verticalMinAngle, _verticalMaxAngle);
            
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