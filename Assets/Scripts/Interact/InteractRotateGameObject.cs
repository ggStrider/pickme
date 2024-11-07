using DG.Tweening;
using UnityEngine;

namespace Interact
{
    public class InteractRotateGameObject : MonoBehaviour, IInteract
    {
        [SerializeField] private Vector3 _endRotate;
        [SerializeField] private Transform _target;

        private Vector3 _firstRotation;
        
        private void Start()
        {
            if (_target != null) return;
            _target = transform;

            _firstRotation = _endRotate;
        }

        public void Interact()
        {
            _target.DORotate(_endRotate, 1f);
            _endRotate += _firstRotation;
        }
    }
}