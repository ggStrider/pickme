using UnityEngine;

namespace Interact
{
    public class InteractDebugLog : IInteract
    {
        public void Interact(bool isPressing)
        {
            if (!isPressing) return;
            Debug.Log("Interacted!");
        }
    }
}
