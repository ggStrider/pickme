using UnityEngine;
using System;

namespace UI.Menu.AnimateMethods
{
    public abstract class AnimateMethod : ScriptableObject
    {
        public abstract void AnimateMenuCategory(RectTransform lastMenuParent, RectTransform newMenuParent,
            Action onStart, Action onCompleteAction);
    }
}