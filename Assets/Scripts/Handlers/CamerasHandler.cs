using System.Collections.Generic;
using UnityEngine;

using Cinemachine;
using Handlers.Observer;

namespace Handlers
{
    public class CamerasHandler : MonoBehaviour
    {
        private List<INewCameraSet> _newCameraSetsObservers = new List<INewCameraSet>();
        private List<IPlayerCameraSet> _playerCameraSetObservers = new List<IPlayerCameraSet>();
        
        private CinemachineVirtualCamera _playerCamera;
        
        private CinemachineVirtualCamera _currentCamera;
        
        public static CamerasHandler Instance { get; private set; }

        public void Initialize(CinemachineVirtualCamera playerCamera)
        {
            _playerCamera = playerCamera;
            _currentCamera = _playerCamera;
            
            if(Instance == null) Instance = this;
            else Destroy(this);

            var cameras = FindObjectsOfType<CinemachineVirtualCamera>();
            foreach (var currentCamera in cameras)
            {
                currentCamera.enabled = false;
            }
            _playerCamera.enabled = true;
        }

        [ContextMenu("Return")]
        public void ReturnToPlayerCamera()
        {
            if (_currentCamera == _playerCamera) return;
            
            _currentCamera.enabled = false;
            
            _currentCamera = _playerCamera;
            _playerCamera.enabled = true;
            
            NotifyPlayerCameraSetObservers();
        }

        public void SwitchCamera(CinemachineVirtualCamera newCamera)
        {
            if (newCamera == _playerCamera)
            {
                ReturnToPlayerCamera();
                return;
            }
            _currentCamera = newCamera;
            
            _currentCamera.enabled = true;
            _playerCamera.enabled = false;

            NotifyNewCameraSetObservers();
        }

        #region Subscriptions

        public void SubscribeToNewCameraSet(INewCameraSet observer)
        {
            if(_newCameraSetsObservers.Contains(observer)) return;
            _newCameraSetsObservers.Add(observer);
        }

        public void SubscribeToPlayerCameraSet(IPlayerCameraSet observer)
        {
            if(_playerCameraSetObservers.Contains(observer)) return;
            _playerCameraSetObservers.Add(observer);
        }

        private void NotifyNewCameraSetObservers()
        {
            if(_newCameraSetsObservers.Count == 0) return;
            foreach (var observer in _newCameraSetsObservers)
            {
                observer.OnNewCameraSet();
            }
        }

        private void NotifyPlayerCameraSetObservers()
        {
            if(_playerCameraSetObservers.Count == 0) return;
            foreach (var observer in _playerCameraSetObservers)
            {
                observer.OnPlayerCameraSet();
            }
        }

        private void OnDestroy()
        {
            _playerCameraSetObservers.Clear();
            _newCameraSetsObservers.Clear();
        }

        #endregion
    }
}