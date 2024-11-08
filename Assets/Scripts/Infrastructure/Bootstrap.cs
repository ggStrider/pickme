using UnityEngine;
using UnityEngine.UI;

using Dialogue;
using Hover;
using Player;
using UI.Crosshair;
using Audio;
using Handlers;

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

            var playerInput = FindObjectOfType<InputReader>();
            playerInput.Initialize(dialogueManager);

            var playerSystem = FindObjectOfType<PlayerSystem>();
            playerSystem.Initialize(dialogueManager);
            
            var crosshairManager = GetComponent<CrosshairManager>();
            crosshairManager.Initialize(_crosshairImage);

            var hoveredCrosshair = FindObjectsOfType<HoveredChangeCrosshair>();
            InitializeCrosshairHovered(crosshairManager, hoveredCrosshair);
            
            var playerCameraRotate = FindObjectOfType<PlayerCameraRotate>();
            playerCameraRotate.Initialize(dialogueManager);
            
            var cameraFocusHandler = playerSystem.gameObject.GetComponent<CameraFocusHandler>();
            cameraFocusHandler.Initialize(dialogueManager);
            
            var dialogues = FindObjectsOfType<DialogueStart>();
            InitializeDialogues(dialogues, dialogueManager, cameraFocusHandler);
        }

        private void Start()
        {
            var audioManager = GetComponent<AudioManager>();
            audioManager.Initialize(_audioManagerPlayOneShotSource);
        }

        private static void InitializeDialogues(DialogueStart[] dialogues, DialogueManager dialogueManager,
            CameraFocusHandler playerCameraHandler)
        {
            foreach (var dialogue in dialogues)
            {
                dialogue.Initialize(dialogueManager, playerCameraHandler);
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