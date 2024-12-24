using UnityEngine;

using TMPro;

using System.Collections;
using System.Collections.Generic;
using Dialogue.Observers;
using Player;
using Zenject;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dialogueText;
        
        private readonly Queue<DialogueData> _dialoguesData = new Queue<DialogueData>();
        
        private readonly List<IDialogueStarted> _dialogueStartedObservers = new List<IDialogueStarted>();
        private readonly List<IDialogueEnded> _dialogueEndedObservers = new List<IDialogueEnded>();
        
        private bool _busy;
        private int _lineIndex;

        private InputReader _inputReader;
        
        [Inject]
        private void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
            _inputReader.OnLeftMousePressStateChanged += state =>
            {
                if (state) NextLine();
            };
        }
        
        public void GetDialogueData(DialogueData dialogueData)
        {
            _dialoguesData.Enqueue(dialogueData);
            NotifyDialogueStarted();
            
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
            
            // For skipping animation
            if (_dialogueText.text != data.Lines[_lineIndex])
            {
                StopAllCoroutines();
                _dialogueText.text = data.Lines[_lineIndex];
            }
            
            // For next line
            else if (data.Lines.Length - 1 > _lineIndex)
            {
                _lineIndex++;
                StartCoroutine(AnimateLine());
            }

            // For next data
            else
            {
                _dialoguesData.Dequeue();
                _lineIndex = 0;

                if (_dialoguesData.Count == 0)
                {
                    _busy = false;
                    _dialogueText.text = string.Empty;

                    NotifyDialoguesEnded();
                }
                else
                {
                    StartCoroutine(AnimateLine());
                    NotifyDialogueStarted();
                }
            }
        }

        #region Observers

        public void SubscribeDialogueStarted(IDialogueStarted observer)
        {
            if (_dialogueStartedObservers.Contains(observer)) return;
            _dialogueStartedObservers.Add(observer);
        }

        private void NotifyDialogueStarted()
        {
            if(_dialogueStartedObservers.Count == 0) return;
            
            foreach (var observer in _dialogueStartedObservers)
            {
                observer.OnDialogueStarted(_dialoguesData.Peek().CanPlayerUseControls);
            }
        }

        public void SubscribeDialogueEnded(IDialogueEnded observer)
        {
            if (_dialogueEndedObservers.Contains(observer)) return;
            _dialogueEndedObservers.Add(observer);
        }

        private void NotifyDialoguesEnded()
        {
            if (_dialogueEndedObservers.Count == 0) return;
            
            foreach (var observer in _dialogueEndedObservers)
            {
                observer.OnAllDialoguesEnded();
            }
        }
        
        #endregion
    }
}
