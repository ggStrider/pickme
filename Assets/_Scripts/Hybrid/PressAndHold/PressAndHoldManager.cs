using UnityEngine;
using UnityEngine.UI;

using Hover;
using Interact;

using System.Collections;
using System.Collections.Generic;

namespace Hybrid.PressAndHold
{
    public class PressAndHoldManager : MonoBehaviour, IInteract, IHovered
    {
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private bool _resetProgressOnFail;
        [SerializeField] private float _addDeltaToProgress = 0.001f;
        
        private bool _pressed;
        private bool _isHovered;
        
        private bool _isCoroutineActive;
        
        private float _currentProgress;
        private float _maxProgress;
        
        private bool _holdActionCompleted;
        
        private readonly List<IPressAndHold> _observers = new List<IPressAndHold>();

        private void Start()
        {
            _maxProgress = _progressSlider.maxValue;
        }

        public void Interact(bool isPressing)
        {
            if(_holdActionCompleted) return;
            _pressed = isPressing;

            if (isPressing)
            {
                if(!_isCoroutineActive) StartCoroutine(ChangeProgress());
            }
            else
            {
                ResetProgress();
            }
        }

        private void ResetProgress()
        {
            if(_holdActionCompleted) return;
            if(!_resetProgressOnFail) return;
            
            _currentProgress = 0;
            _progressSlider.value = _currentProgress;
        }

        public void Hovered()
        {
            if(_holdActionCompleted) return;

            _isHovered = true;
        }

        public void UnHovered()
        {
            if(_holdActionCompleted) return;
            
            _pressed = false;
            _isHovered = false;

            ResetProgress();
        }

        private IEnumerator ChangeProgress()
        {
            _isCoroutineActive = true;
            while (IsHoldingCrosshairOnObject() && _currentProgress < _maxProgress)
            {
                _currentProgress = Mathf.Clamp(_currentProgress + _addDeltaToProgress, 0, _maxProgress);
                _progressSlider.value = _currentProgress;

                yield return null;
            }
            _isCoroutineActive = false;
            
            if (!(_currentProgress >= _maxProgress)) yield break;
            
            _holdActionCompleted = true;
            
            _isHovered = false;
            _pressed = false;
            NotifyAllObservers();
        }

        /// <summary>
        /// Invokes this method when progress = maxValue
        /// </summary>
        private void NotifyAllObservers()
        {
            if(_observers.Count == 0) return;
            foreach (var observer in _observers)
            {
                observer.Completed();
            }
        }

        private bool IsHoldingCrosshairOnObject()
        {
            return _pressed && _isHovered;
        }

        #region Subscription

        public void SubscribeObserver(IPressAndHold observer)
        {
            if(_observers.Contains(observer)) return;
            _observers.Add(observer);
        }

        private void OnDisable()
        {
            _observers.Clear();
        }

        #endregion
    }
}
