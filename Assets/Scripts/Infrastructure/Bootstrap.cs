using UnityEngine;
using UnityEngine.UI;

using Dialogue;
using Hover;
using Player;
using UI.Crosshair;
using Audio;

namespace Infrastructure
{
    [RequireComponent(typeof(DialogueManager))]
    [RequireComponent(typeof(CrosshairManager))]
    [RequireComponent(typeof(AudioManager))]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Image _crosshairImage;
        [SerializeField] private AudioSource _audioManagerPlayOneShotSource;
        
        private void Awake()
        {
            var dialogueManager = GetComponent<DialogueManager>();
            var dialogues = FindObjectsOfType<DialogueStart>();

            InitializeDialogues(dialogues, dialogueManager);

            var playerInput = FindObjectOfType<InputReader>();
            playerInput.Initialize(dialogueManager);

            var playerSystem = FindObjectOfType<PlayerSystem>();
            playerSystem.Initialize();
            
            var crosshairManager = GetComponent<CrosshairManager>();
            crosshairManager.Initialize(_crosshairImage);

            var hoveredCrosshair = FindObjectsOfType<HoveredChangeCrosshair>();
            InitializeCrosshairHovered(crosshairManager, hoveredCrosshair);
        }

        private void Start()
        {
            var audioManager = GetComponent<AudioManager>();
            audioManager.Initialize(_audioManagerPlayOneShotSource);
        }

        private static void InitializeDialogues(DialogueStart[] dialogues, DialogueManager dialogueManager)
        {
            foreach (var dialogue in dialogues)
            {
                dialogue.Initialize(dialogueManager);
            }
        }

        private static void InitializeCrosshairHovered(CrosshairManager manager, HoveredChangeCrosshair[] changers)
        {
            foreach (var changer in changers)
            {
                changer.Initialize(manager);
            }
        }
    }
}