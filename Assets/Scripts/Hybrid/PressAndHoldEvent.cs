using System.Collections;
using Hover;
using Interact;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hybrid
{
    public class PressAndHoldEvent : MonoBehaviour, IInteract, IHovered
    {
        [SerializeField] private bool _pressed;
        [SerializeField] private bool _isHovered;
        [SerializeField] private Slider _slider;
        
        [SerializeField] private UnityEvent _onComplete;
        [SerializeField] private float _progress;
        private float _maxProgress;
        
        private bool _completed;

        private void Start()
        {
            _maxProgress = _slider.maxValue;
        }

        public void Interact(bool isPressing)
        {
            _pressed = isPressing;

            if (isPressing)
            {
                StartCoroutine(Add());
            }
            else
            {
                ResetProgress();
            }
        }

        private void ResetProgress()
        {
            if(_completed) return;
            
            _progress = 0;
            _slider.value = _progress;
        }

        public void Hovered()
        {
            _isHovered = true;
        }

        public void UnHovered()
        {
            _pressed = false;
            _isHovered = false;

            ResetProgress();
        }

        private IEnumerator Add()
        {
            while (IsHoldingCrosshairOnObject() && _progress < _maxProgress)
            {
                _progress = Mathf.Clamp(_progress + Time.deltaTime, 0, _maxProgress);
                _slider.value = _progress;

                yield return null;
            }

            if (_progress >= _maxProgress)
            {
                _onComplete?.Invoke();
                _completed = true;
            }
        }

        private bool IsHoldingCrosshairOnObject()
        {
            return _pressed && _isHovered;
        }
    }
}
