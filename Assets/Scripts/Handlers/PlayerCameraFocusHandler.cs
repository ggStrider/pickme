using UnityEngine;

using Cinemachine;
using DG.Tweening;

using Dialogue;
using Dialogue.Observers;

using Handlers.Settings.CameraFocusHandler;
using Handlers.Observer;

using System.Collections.Generic;
using Zenject;

namespace Handlers
{
    public class PlayerCameraFocusHandler : MonoBehaviour, IDialogueEnded
    {
        [SerializeField] private CameraFocusHandlerSettings _settings;

        private List<IFocused> _observers = new List<IFocused>();
        
        private Transform _camera;
        private CinemachineVirtualCamera _vCamera;

        private float _defaultFOV;

        [Inject]
        private void Construct(CinemachineVirtualCamera virtualCamera, DialogueManager dialogueManager)
        {
            _camera = virtualCamera.transform;
            _vCamera = virtualCamera;

            dialogueManager.SubscribeDialogueEnded(this);
            
            _defaultFOV = _vCamera.m_Lens.FieldOfView;
        }
        
        public void LookAtAndFocus(Transform target)
        {
            _camera.DOLookAt(target.position, _settings.TimeToRotate)
                .OnComplete(() => NotifyAllObservers(_camera.rotation.eulerAngles));
            
            Focus(_settings.NewFov);
        }

        private void NotifyAllObservers(Vector3 newRotation)
        {
            if(_observers.Count == 0) return;
            
            foreach (var observer in _observers)
            {
                observer.OnCameraFocused(newRotation);
            }
        }

        public void Focus(float newFov)
        {
            DOVirtual.Float(_vCamera.m_Lens.FieldOfView, newFov, _settings.TimeToFocus, value => 
            {
                _vCamera.m_Lens.FieldOfView = value;
            }).SetEase(Ease.InOutQuad);
        }

        public void ResetFov()
        {
            Focus(_defaultFOV);
        }

        public void OnAllDialoguesEnded()
        {
            ResetFov();
        }

        #region Subscriptions

        public void Subscribe(IFocused observer)
        {
            if(_observers.Contains(observer)) return;
            _observers.Add(observer);
        }

        private void OnDestroy()
        {
            _observers.Clear();
        }

        #endregion
    }
}