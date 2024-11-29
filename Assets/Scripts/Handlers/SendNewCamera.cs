using UnityEngine;
using Cinemachine;

using Interact;

namespace Handlers
{
    public class SendNewCamera : MonoBehaviour, IInteract
    {
        [SerializeField] private CinemachineVirtualCamera _cameraToEnable;
        
        public void Interact(bool isPressing)
        {
            CamerasHandler.Instance.SwitchCamera(_cameraToEnable);
        }
    }
}