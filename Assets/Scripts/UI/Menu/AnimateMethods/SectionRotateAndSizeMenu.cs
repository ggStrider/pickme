using UnityEngine;
using DG.Tweening;

using System;

namespace UI.Menu.AnimateMethods
{
    [CreateAssetMenu(fileName = "New AnimateByRotation", 
        menuName = "Thenexy/UI/Menu/Animate by Rotation and Size")]
    public class SectionRotateAndSizeMenu : AnimateMethod
    {
        [SerializeField] private float _animationFragmentDuration;
        [SerializeField] private Vector3 _rotateAxis = new Vector3(0, 0, 360);
        [SerializeField] private Vector3 _newMenuEndSize = Vector3.one;
        
        public override void AnimateMenuCategory(RectTransform lastMenuParent, RectTransform newMenuParent, Action onStart,
            Action onCompleteAction)
        {
            onStart?.Invoke();
            
            var sequence = DOTween.Sequence();

            // Hide
            sequence.Append(lastMenuParent.DORotate(_rotateAxis, _animationFragmentDuration, RotateMode.WorldAxisAdd));
            sequence.Join(lastMenuParent.DOScale(Vector3.zero, _animationFragmentDuration));

            newMenuParent.localScale = Vector3.zero;
            
            // Show
            sequence.Append(newMenuParent.DORotate(_rotateAxis, _animationFragmentDuration, RotateMode.WorldAxisAdd));
            sequence.Join(newMenuParent.DOScale(_newMenuEndSize, _animationFragmentDuration));
            
            sequence.Play();
            sequence.OnComplete(() => onCompleteAction?.Invoke());
        }
    }
}