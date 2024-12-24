using UnityEngine;

namespace Events
{
    public class DotChecker : MonoBehaviour
    {
        [SerializeField] private Transform _playerCamera;

        public void Initialize(Transform playerCamera)
        {
            _playerCamera = playerCamera;
        }
        
        private void Update()
        {
            Vector3 toOther = Vector3.Normalize(transform.position - _playerCamera.position);
            Vector3 forward = _playerCamera.TransformDirection(Vector3.forward);
            
            var dot = Vector3.Dot(forward, toOther);
            
            Debug.Log(dot);
        }
    }
}
