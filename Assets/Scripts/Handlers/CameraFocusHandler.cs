using UnityEngine;

using Cinemachine;
using DG.Tweening;

using Dialogue;
using Dialogue.Observers;
using Handlers.Settings.CameraFocusHandler;

namespace Handlers
{
    public class CameraFocusHandler : MonoBehaviour, IDialogueEnded
    {
        [SerializeField] private float _timeToResetFov;
        [SerializeField] private CameraFocusHandlerSettings _settings;

        private Transform _camera;
        private CinemachineVirtualCamera _vCamera;

        private float _defaultFOV;

        public void GetCamera(CinemachineVirtualCamera virtualCamera)
        {
            _camera = virtualCamera.transform;
            _vCamera = virtualCamera;
            
            _defaultFOV = _vCamera.m_Lens.FieldOfView;
        }
        
        public void Initialize(DialogueManager dialogueManager)
        { 
            dialogueManager.SubscribeDialogueEnded(this);
        }
        
        public void LookAtAndFocus(Transform target)
        {
            _camera.DOLookAt(target.position, _settings.TimeToRotate);

            Focus();
        }

        public void Focus()
        {
            DOVirtual.Float(_vCamera.m_Lens.FieldOfView, _settings.NewFov, _settings.TimeToFocus, value => 
            {
                _vCamera.m_Lens.FieldOfView = value;
            }).SetEase(Ease.InOutQuad);
        }

        public void ResetFov()
        {
            Focus();
        }

        public void OnAllDialoguesEnded()
        {
            ResetFov();
        }
    }
}