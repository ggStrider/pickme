using UnityEngine;

using Dialogue;

namespace Interact
{
    [RequireComponent(typeof(DialogueStart))]
    public class InteractDialogue : MonoBehaviour, IInteract
    {
        private DialogueStart _dialogue;

        private void Start()
        {
            _dialogue = GetComponent<DialogueStart>();
        }

        public void Interact(bool isPressing)
        {
            if (!isPressing) return;

            _dialogue.OnStartDialogue();
        }
    }
}
