using UnityEngine;

namespace UI.Images.FaderSettings
{
    [CreateAssetMenu(fileName = "New Fader Settings", menuName = "Thenexy/Fader Settings")]
    public class FaderSettings : ScriptableObject
    {
        public float FadeInTime;
        public float FadeOutTime;
    }
}