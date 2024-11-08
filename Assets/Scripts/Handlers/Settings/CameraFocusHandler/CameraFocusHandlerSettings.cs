using UnityEngine;

namespace Handlers.Settings.CameraFocusHandler
{
    [CreateAssetMenu(fileName = "CameraFocusHandlerSettings", menuName = "Thenexy/Camera Focus Handler")]
    public class CameraFocusHandlerSettings : ScriptableObject
    {
        public float TimeToRotate;
        
        public float TimeToFocus;
        public float NewFov;
    }
}
