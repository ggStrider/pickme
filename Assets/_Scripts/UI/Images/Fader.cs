using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.Images
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Image _blackScreen;
        [SerializeField] private FaderSettings.FaderSettings _settings;
        
        [ContextMenu("Fade In")]
        public void FadeIn()
        {
            _blackScreen.DOFade(0, _settings.FadeInTime);
        }
        
        [ContextMenu("Fade Out")]
        public void FadeOut()
        {
            _blackScreen.DOFade(0, _settings.FadeOutTime);
        }
    }
}
