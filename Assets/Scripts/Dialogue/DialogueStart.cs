using UnityEngine;

using Handlers;

namespace Dialogue
{
    public class DialogueStart : MonoBehaviour
    {
        [SerializeField] private DialogueData _dialogue;
        
        [Header("If null - no focusing")]
        [SerializeField] private Transform _target;

        private PlayerCameraFocusHandler _playerFocusHandler;
        private DialogueManager _dialogueManager;

        public void Initialize(DialogueManager dialogueManager, PlayerCameraFocusHandler playerPlayerCameraFocusHandler)
        {
            _dialogueManager = dialogueManager;
            _playerFocusHandler = playerPlayerCameraFocusHandler;
        }

        [ContextMenu("Start dialogue")]
        public void OnStartDialogue()
        {
            _dialogueManager.GetDialogueData(_dialogue);

            if (_playerFocusHandler == null || _target == null) return;
            _playerFocusHandler.LookAtAndFocus(_target);
        }
    }
}