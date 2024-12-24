using UnityEngine;

namespace Interact
{
    public class InteractToggleLight : MonoBehaviour, IInteract
    {
        [SerializeField] private Light _lightToToggle;
        [SerializeField] private float _enabledIntensity = 3;

        [Space, Header("Play sound")]
        [SerializeField] private AudioSource _soundSource;
        
        // Refactor into scriptable object
        [SerializeField] private AudioClip _enabledClip;
        [SerializeField] private AudioClip _disabledClip;
        
        private bool _enabled;

        private void Start()
        {
            if(_lightToToggle == null) Debug.Log("Light to toggle is null!" + gameObject.name);
            _enabled = _lightToToggle.intensity > 0;
        }
        
        public void Interact(bool isPressing)
        {
            if(!isPressing) return;

            _enabled = !_enabled;
            _lightToToggle.intensity = _enabled ? _enabledIntensity : 0;
            
            _soundSource.PlayOneShot(_enabledClip ? _enabledClip : _disabledClip);
        }
    }
}
