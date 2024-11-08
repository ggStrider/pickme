using UnityEngine;

using Dialogue;
using Dialogue.Observers;

namespace Player
{
    public class PlayerCameraRotate : MonoBehaviour, IDialogueStarted, IDialogueEnded
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private bool _canRotate = true;
        
        [Space]
        [SerializeField] private float _sensitivity = 0.3f;
        
        [Space]
        [SerializeField] private float _verticalMinAngle = -80f;
        [SerializeField] private float _verticalMaxAngle = 80f;
        
        private float _rotationX;

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

        public void Initialize(DialogueManager dialogueManager)
        {
            dialogueManager.SubscribeDialogueStarted(this);
            dialogueManager.SubscribeDialogueEnded(this);
        }

        /// <summary>
        /// Method to rotate player and camera
        /// </summary>
        /// <param name="lookRotation">rotate vector</param>
        public void SetLookRotation(Vector2 lookRotation)
        {
            if (!_canRotate) return;
            var finalRotationVector = lookRotation * _sensitivity;
            
            var xRotateDelta = finalRotationVector.x;
            transform.Rotate(Vector3.up * xRotateDelta);

            _rotationX -= finalRotationVector.y;
            _rotationX = Mathf.Clamp(_rotationX, _verticalMinAngle, _verticalMaxAngle);

            _camera.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        }

        public void OnDialogueStarted(bool canControl)
        {
            _canRotate = canControl;
        }

        public void OnAllDialoguesEnded()
        {
            _canRotate = true;
        }
    }
}