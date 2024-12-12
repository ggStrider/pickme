using UnityEngine;
using DG.Tweening;

using System;

namespace UI.Menu.AnimateMethods
{
    [CreateAssetMenu(fileName = "New AnimateBySize", 
        menuName = "Thenexy/UI/Menu/Animate by Size")]
    public class SectionSizeMenu : AnimateMethod
    {
        [SerializeField] private float _animationFragmentDuration;
        [SerializeField] private Vector3 _newMenuParentEndSize = Vector3.one;
        
        [SerializeField] private Ease _easeType;
        
        public override void AnimateMenuCategory(RectTransform lastMenuParent, RectTransform newMenuParent, 
            Action onStart, Action onCompleteAction)
        {
            onStart?.Invoke();
            
            var sequence = DOTween.Sequence();

            // Hide
            sequence.Append(lastMenuParent.DOScale(Vector3.zero, _animationFragmentDuration).SetEase(_easeType));

            newMenuParent.localScale = Vector3.zero;
            
            // Show
            sequence.Append(newMenuParent.DOScale(_newMenuParentEndSize, _animationFragmentDuration).SetEase(_easeType));
            
            sequence.Play();
            sequence.OnComplete(() => onCompleteAction?.Invoke());
        }
    }
}
