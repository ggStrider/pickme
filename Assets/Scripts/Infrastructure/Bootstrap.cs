using AnimationControllers;
using UnityEngine;
using UnityEngine.UI;

using Dialogue;
using Hover;
using Player;
using UI.Crosshair;
using Audio;
using Handlers;
using Hybrid.TakeObject;

namespace Infrastructure
{
    [RequireComponent(typeof(DialogueManager))]
    [RequireComponent(typeof(CrosshairManager))]
    [RequireComponent(typeof(AudioManager))]
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Image _crosshairImage;
        [SerializeField] private AudioSource _audioManagerPlayOneShotSource;
        [SerializeField] private Transform _playerItemPlace;
        
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
            
            var cameraFocusHandler = playerSystem.gameObject.GetComponent<PlayerCameraFocusHandler>();
            cameraFocusHandler.Initialize(dialogueManager);
            
            var playerCameraRotate = FindObjectOfType<PlayerCameraRotate>();
            playerCameraRotate.Initialize(dialogueManager, cameraFocusHandler);
            
            var dialogues = FindObjectsOfType<DialogueStart>();
            InitializeDialogues(dialogues, dialogueManager, cameraFocusHandler);

            var takeableObjects = FindObjectsOfType<TakeableGameObject>();
            InitializeTakeableObjects(takeableObjects);
            
            var audioManager = GetComponent<AudioManager>();
            audioManager.Initialize(_audioManagerPlayOneShotSource);

            var dialogueAnimationToggler = FindObjectOfType<DialogueAnimationToggler>();
            dialogueAnimationToggler.Initialize(dialogueManager);
        }

        private void InitializeTakeableObjects(TakeableGameObject[] takeableObjects)
        {
            foreach (var takeableObject in takeableObjects)
            {
                takeableObject.Initialize(_playerItemPlace);
            }
        }

        private static void InitializeDialogues(DialogueStart[] dialogues, DialogueManager dialogueManager,
            PlayerCameraFocusHandler playerPlayerCameraHandler)
        {
            foreach (var dialogue in dialogues)
            {
                dialogue.Initialize(dialogueManager, playerPlayerCameraHandler);
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