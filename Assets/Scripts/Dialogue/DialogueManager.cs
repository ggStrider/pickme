using UnityEngine;
using TMPro;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Dialogue
{
    [RequireComponent(typeof(DialogueUI))]
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dialogueText;
        
        private readonly Queue<DialogueData> _dialoguesData = new Queue<DialogueData>();
        private DialogueUI _dialogueUI;

        private bool _busy;
        private int _lineIndex;
        
        private void Start()
        {
            _dialogueUI = GetComponent<DialogueUI>();
        }

        public void GetDialogueData(DialogueData dialogueData)
        {
            _dialoguesData.Enqueue(dialogueData);
            
            if(_busy) return;
            _busy = true;
            _lineIndex = 0;

            StartCoroutine(AnimateLine(_dialoguesData.Peek()));
        }

        private IEnumerator AnimateLine(DialogueData data)
        {
            _dialogueText.text = string.Empty;
            var chars = data.Lines[_lineIndex].ToCharArray();
            
            foreach (var currentChar in chars)
            {
                _dialogueText.text += currentChar;

                if (currentChar != ' ') 
                    yield return new WaitForSeconds(data.CharDelay);
            }
        }

        // lmb
        public void NextLine(InputAction.CallbackContext context)
        {
            if(_dialoguesData.Count == 0) return;

            if (_dialoguesData.Peek().Lines.Length - 1 > _lineIndex)
            {
                _lineIndex++;
                StartCoroutine(AnimateLine(_dialoguesData.Peek()));
            }
            else
            {
                _dialoguesData.Dequeue();
                
                _dialogueText.text = string.Empty;
                if (_dialoguesData.Count == 0) return;

                _lineIndex = 0;
                StartCoroutine(AnimateLine(_dialoguesData.Peek()));
            }
        }
    }
}
