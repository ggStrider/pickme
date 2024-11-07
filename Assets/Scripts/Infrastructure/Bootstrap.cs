using UnityEngine;

using Dialogue;
using Player;

namespace Infrastructure
{
    [RequireComponent(typeof(DialogueManager))]
    public class Bootstrap : MonoBehaviour
    {
        private void Awake()
        {
            var dialogueManager = GetComponent<DialogueManager>();
            var dialogues = FindObjectsOfType<DialogueStart>();

            InitializeDialogues(dialogues, dialogueManager);

            var playerInput = FindObjectOfType<InputReader>();
            playerInput.Initialize(dialogueManager);

            var playerSystem = FindObjectOfType<PlayerSystem>();
            playerSystem.Initialize();
        }

        private static void InitializeDialogues(DialogueStart[] dialogues, DialogueManager dialogueManager)
        {
            foreach (var dialogue in dialogues)
            {
                dialogue.Initialize(dialogueManager);
            }
        }
    }
}