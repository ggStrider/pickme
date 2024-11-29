using UnityEngine;

namespace Interact
{
    public class InteractDebugLog : InteractCompleteTask
    {
        public override void Interact(bool isPressing)
        {
            if (!isPressing) return;
            
            Debug.Log("Interacted!");
            base.Interact(true);
        }
    }
}
