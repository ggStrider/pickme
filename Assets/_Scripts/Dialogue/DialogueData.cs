using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Thenexy/Dialogue")]
    public class DialogueData : ScriptableObject
    {
        [TextArea(2, 3)]
        public string[] Lines;

        public float CharDelay = 0.05f;

        [Space]
        public bool CanPlayerUseControls;
    }
}
