using UnityEngine;

namespace Environment
{
    public class LookAtObject : MonoBehaviour
    {
        [SerializeField] private bool _createParentWith0Coordinates;
        
        [SerializeField] private Vector3 _offset;
        
        [Space]
        [SerializeField] private Transform _targetToLookAt;
        [SerializeField] private Transform _targetToRotate;
        
        [ContextMenu("st")]
        private void Start()
        {
            _targetToRotate ??= transform;
        }
        
        private void LateUpdate()
        {
            _targetToRotate.LookAt(_targetToLookAt);
        }

        public void _SetCanLook(bool canLook)
        {
            enabled = canLook;
        }
    }
}
