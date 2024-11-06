using Dialogue;
using UnityEngine;

namespace Player
{
    public class InputReader : MonoBehaviour
    {
        private PlayerMap _playerMap;

        public void Initialize(DialogueManager dialogueManager)
        {
            _playerMap = new PlayerMap();

            _playerMap.Main.LMB.started += dialogueManager.NextLine;
            
            _playerMap.Enable();
        }
    }
}