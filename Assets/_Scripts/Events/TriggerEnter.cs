using UnityEngine;
using UnityEngine.Events;

using System;

namespace Events
{
    public class TriggerEnter : MonoBehaviour
    {
        [SerializeField] private string _tag = "Player";
        [SerializeField] private EnterEvent _enterEvent;

        private void OnTriggerEnter(Collider other)
        {
            if(_tag == string.Empty) return;
            if(!other.CompareTag(_tag)) return;
            
            _enterEvent?.Invoke(other.gameObject);
        }
        
        [Serializable]
        public class EnterEvent : UnityEvent<GameObject> { }
    }
}
