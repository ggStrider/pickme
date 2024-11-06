using UnityEngine;

namespace Dialogue
{
    public class DialogueStart : MonoBehaviour
    {
        [SerializeField] private DialogueData _dialogue;
        private DialogueManager _dialogueManager;

        public void Initialize(DialogueManager dialogueManager)
        {
            _dialogueManager = dialogueManager;
        }

        [ContextMenu("Start dialogue")]
        public void OnStartDialogue()
        {
            _dialogueManager.GetDialogueData(_dialogue);
        }
    }
}