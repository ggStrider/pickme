using Data;
using GetObjects;
using Interact;
using Player.Main;
using UnityEngine;
using Zenject;

namespace Player.Additional
{
    public class InteractWithPropSystem : MonoBehaviour
    {
        [SerializeField] private Transform _outFrom;
        [SerializeField] private float _checkDistance;
        private IInteractProp _currentInHand;
        
        private GetGameObjectByCast _getGameObjectByCast;
        
        private InputReader _inputReader;

        [Inject]
        public void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
            _inputReader.OnLeftMousePressStateChanged += InteractWithProp;
        }

        private void OnDestroy()
        {
            _inputReader.OnLeftMousePressStateChanged -= InteractWithProp;
        }

        private void Start()
        {
            _getGameObjectByCast = new GetGameObjectByCast();
        }
        
        private void InteractWithProp(bool isPressing)
        {
            if (_currentInHand != null && !isPressing)
            {
                _currentInHand.Interact(false);

                _currentInHand = null;
                return;
            }
            
            var prop = _getGameObjectByCast.GetByRay(_outFrom.position,
                    _outFrom.forward, _checkDistance, DefaultData.TriggerLayer)?
                .GetComponent<IInteractProp>();
            
            if(prop is null) return;
            
            _currentInHand = prop;
            prop.Interact(isPressing);
        }
    }
}