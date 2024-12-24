using UnityEngine;

namespace UI.Crosshair
{
    [CreateAssetMenu(fileName = "CrosshairSettings", menuName = "Thenexy/Crosshair Settings")]
    public class CrosshairSettings : ScriptableObject
    {
        public Sprite Crosshair;
        public Color CrosshairColor = Color.white;
        public Vector2 CrosshairSize;
    }
}