using UnityEngine;

using GetObjects;
using Data;
using Interact;
using Zenject;

namespace Player
{
    public class InteractSystem : MonoBehaviour
    {
        [SerializeField] private Transform _playerCamera;
        [SerializeField] private float _interactRayDistance;

        private GetGameObjectByCast _getGameObjectByCast;

        private InputReader _inputReader;

        [Inject]
        private void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
            
            _inputReader.OnInteractButtonPressStateChanged += TryInteract;
        }

        private void OnDestroy()
        {
            if(_inputReader != null) _inputReader.OnInteractButtonPressStateChanged -= TryInteract;
        }

        private void Awake()
        {
            _getGameObjectByCast = new GetGameObjectByCast();
        }
        
        private void TryInteract(bool isPressingInteractButton)
        {
            var objectInRay = _getGameObjectByCast.GetByRay(_playerCamera.position,
                _playerCamera.forward, _interactRayDistance, DefaultData.TriggerLayer);

            objectInRay?.GetComponent<IInteract>()?.Interact(isPressingInteractButton);
        }
    }
}