using UnityEngine;

namespace Interact
{
    public class InteractDebugLog : MonoBehaviour, IInteract
    {
        public void Interact(bool isPressing)
        {
            Debug.Log("Interacted!");
        }
    }
}
