using UnityEngine;

namespace Hovered
{
    [RequireComponent(typeof(Outline))]
    public class HoveredOutline : MonoBehaviour, IHovered
    {
        private Outline _outline;
        private void Start()
        {
            _outline = GetComponent<Outline>();
            
            _outline.enabled = false;
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
        }
        
        public void Hovered()
        {
            _outline.enabled = true;
        }

        public void UnHovered()
        {
            _outline.enabled = false;
        }
    }
}