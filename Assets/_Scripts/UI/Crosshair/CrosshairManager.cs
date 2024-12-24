using UnityEngine;
using UnityEngine.UI;

namespace UI.Crosshair
{
    public class CrosshairManager : MonoBehaviour
    {
        [SerializeField] private CrosshairSettings _currentCrosshair;
        [SerializeField] private CrosshairSettings _defaultCrosshair;
        
        private Image _crosshair;

        public void Initialize(Image crosshairImage)
        {
            _crosshair = crosshairImage;
            ResetCrosshair();
        }

        public void ResetCrosshair() => SetNewCrosshair(_defaultCrosshair);

        public void SetNewCrosshair(CrosshairSettings newCrosshair)
        {
            _currentCrosshair = newCrosshair;
            
            _crosshair.sprite = _currentCrosshair.Crosshair;
            _crosshair.transform.localScale = _currentCrosshair.CrosshairSize;
        }
    }
}
