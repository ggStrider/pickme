using UnityEngine;

using TMPro;

using System.Collections;
using System.Collections.Generic;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dialogueText;
        
        private readonly Queue<DialogueData> _dialoguesData = new Queue<DialogueData>();

        private bool _busy;
        private int _lineIndex;
        
        public void GetDialogueData(DialogueData dialogueData)
        {
            _dialoguesData.Enqueue(dialogueData);
            
            if(_busy) return;
            _busy = true;
            
            StartCoroutine(AnimateLine());
        }

        private IEnumerator AnimateLine()
        {
            _dialogueText.text = string.Empty;
            
            var data = _dialoguesData.Peek();
            foreach (var chars in data.Lines[_lineIndex])
            {
                _dialogueText.text += chars;
                yield return new WaitForSeconds(data.CharDelay);
            }
        }

        public void NextLine()
        {
            if (!_busy) return;
            
            var data = _dialoguesData.Peek();
            
            if (_dialogueText.text != data.Lines[_lineIndex])
            {
                StopAllCoroutines();
                _dialogueText.text = data.Lines[_lineIndex];
            }
            
            else if (data.Lines.Length - 1 > _lineIndex)
            {
                _lineIndex++;
                StartCoroutine(AnimateLine());
            }

            else
            {
                _dialoguesData.Dequeue();
                _lineIndex = 0;

                if (_dialoguesData.Count == 0)
                {
                    _busy = false;
                    _dialogueText.text = string.Empty;
                }
                else
                {
                    StartCoroutine(AnimateLine());
                }
            }
        }
    }
}
