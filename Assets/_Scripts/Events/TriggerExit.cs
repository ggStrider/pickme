using System;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class TriggerExit : MonoBehaviour
    {
        [SerializeField] private string _tag = "Player";
        [SerializeField] private UnityEvent _action;
        
        private void OnTriggerExit(Collider other)
        {
            if (_tag == string.Empty) return;
            if(!other.CompareTag(_tag)) return;
            
            _action?.Invoke();
        }
    }
}
