using AnimationControllers;
using UnityEngine;
using UnityEngine.UI;

using Dialogue;
using Hover;
using Player;
using UI.Crosshair;
using Audio;
using Cinemachine;
using Handlers;
using Hybrid.TakeObject;
using Zenject;

namespace Infrastructure
{
    [RequireComponent(typeof(DialogueManager))]
    [RequireComponent(typeof(CrosshairManager))]
    [RequireComponent(typeof(AudioManager))]
    public class Bootstrap : MonoBehaviour
    {
        // Refactor this class later
        
        [SerializeField] private Image _crosshairImage;
        [SerializeField] private AudioSource _audioManagerPlayOneShotSource;
        [SerializeField] private Transform _playerItemPlace;
        [SerializeField] private CinemachineVirtualCamera _playerVirtualCamera;
        
        private void Awake()
        {
            var camerasHandler = GetComponent<CamerasHandler>();
            camerasHandler.Initialize(_playerVirtualCamera);
            
            var dialogueManager = GetComponent<DialogueManager>();

            var playerSystem = FindObjectOfType<PlayerMovement>();
            
            var crosshairManager = GetComponent<CrosshairManager>();
            crosshairManager.Initialize(_crosshairImage);

            var hoveredCrosshair = FindObjectsOfType<HoveredChangeCrosshair>();
            InitializeCrosshairHovered(crosshairManager, hoveredCrosshair);
            
            var cameraFocusHandler = playerSystem.gameObject.GetComponent<PlayerCameraFocusHandler>();
            
            var dialogues = FindObjectsOfType<DialogueStart>();
            InitializeDialogues(dialogues, dialogueManager, cameraFocusHandler);

            var takeableObjects = FindObjectsOfType<TakeableGameObject>();
            InitializeTakeableObjects(takeableObjects);
            
            var dialogueAnimationToggler = FindObjectOfType<DialogueAnimationToggler>();
            dialogueAnimationToggler.Initialize(dialogueManager);
        }

        private void Start()
        {
            var audioManager = GetComponent<AudioManager>();
            audioManager.Initialize(_audioManagerPlayOneShotSource);
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