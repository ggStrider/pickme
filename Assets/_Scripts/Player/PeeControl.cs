using UnityEngine;

namespace Player
{
    public class PeeControl : MonoBehaviour
    {
        [SerializeField] private Transform _pee;

        [SerializeField] private float _minusClamp = -40;
        [SerializeField] private float _positiveClamp = 40;

        private float _currentYDirection;

        private void Start()
        {
            _currentYDirection = transform.eulerAngles.y;
        }

        public void SetDirection(float yDirection)
        {
            _currentYDirection = Mathf.Clamp(_currentYDirection + yDirection, _minusClamp, _positiveClamp);
            _pee.localRotation = Quaternion.Euler(0, _currentYDirection, 0);
        }
    }
}