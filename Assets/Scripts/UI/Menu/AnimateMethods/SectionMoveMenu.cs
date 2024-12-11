using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Menu.AnimateMethods
{
    [CreateAssetMenu(fileName = "New AnimateByPosition", 
        menuName = "Thenexy/UI/Menu/Animate by Position")]
    public class SectionMoveMenu : AnimateMethod
    {
        [SerializeField] private float _animationFragmentDuration;

        [SerializeField] private float _xHiddenPosition;
        [SerializeField] private float _xShownPosition;
        
        public override void AnimateMenuCategory(RectTransform lastMenuParent, RectTransform newMenuParent, 
            Action onStart, Action onCompleteAction)
        {
            onStart?.Invoke();
            
            newMenuParent.anchoredPosition = new Vector3(_xHiddenPosition, newMenuParent.anchoredPosition.y, 0);

            var sequence = DOTween.Sequence();

            // Hide
            sequence.Append(lastMenuParent.DOAnchorPosX(_xHiddenPosition, _animationFragmentDuration));
            
            // Show
            sequence.Append(newMenuParent.DOAnchorPosX(_xShownPosition, _animationFragmentDuration));
            
            sequence.Play();
            sequence.OnComplete(() => onCompleteAction?.Invoke());
        }
    }
}