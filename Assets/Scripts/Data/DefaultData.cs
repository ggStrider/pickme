using UnityEngine;

namespace Data
{
    public static class DefaultData
    {
        public static readonly LayerMask TriggerLayer = LayerMask.GetMask("Trigger");
        public const float INTERACT_DISTANCE = 3f;
    }
}