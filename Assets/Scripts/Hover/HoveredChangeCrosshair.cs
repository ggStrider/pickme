using UI.Crosshair;
using UnityEngine;

namespace Hover
{
    public class HoveredChangeCrosshair : MonoBehaviour, IHovered
    {
        [SerializeField] private CrosshairSettings _hoveredCrosshair;
        private CrosshairManager _crosshairManager;

        public void Initialize(CrosshairManager manager)
        {
            Debug.Log(manager);
            _crosshairManager = manager;
        }

        public void Hovered()
        {
            _crosshairManager.SetNewCrosshair(_hoveredCrosshair);
        }

        public void UnHovered()
        {
            _crosshairManager.ResetCrosshair();
        }
    }
}