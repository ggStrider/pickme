using UnityEngine;
using UnityEngine.InputSystem;

using Dialogue;

namespace Player
{
    public class InputReader : MonoBehaviour
    {
        private PlayerMap _playerMap;
        private DialogueManager _dialogueManager;

        public void Initialize(DialogueManager dialogueManager)
        {
            _playerMap = new PlayerMap();
            
            _dialogueManager = dialogueManager;
            _playerMap.Main.LMB.started += OnLeftMousePressed;
            
            _playerMap.Enable();
        }

        private void OnLeftMousePressed(InputAction.CallbackContext context)
        {
            _dialogueManager.NextLine();
        }
    }
}