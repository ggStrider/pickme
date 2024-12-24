using UnityEngine;

using Dialogue;
using Dialogue.Observers;

namespace AnimationControllers
{
    public class DialogueAnimationToggler : MonoBehaviour, IDialogueStarted, IDialogueEnded
    {
        private readonly int _showKey = Animator.StringToHash("show");
        
        private Animator _dialogueAnimator;
        
        public void Initialize(DialogueManager dialogueManager)
        {
            dialogueManager.SubscribeDialogueStarted(this);
            dialogueManager.SubscribeDialogueEnded(this);

            _dialogueAnimator = GetComponent<Animator>();
        }

        public void OnDialogueStarted(bool canControl)
        {
            _dialogueAnimator.SetBool(_showKey, true);
        }

        public void OnAllDialoguesEnded()
        {
            _dialogueAnimator.SetBool(_showKey, false);
        }
    }
}
