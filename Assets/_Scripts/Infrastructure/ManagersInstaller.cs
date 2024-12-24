using Dialogue;
using Handlers;
using Player;
using Player.Main;
using UnityEngine;

using Zenject;

namespace Infrastructure
{
    public class ManagersInstaller : MonoInstaller
    {
        [SerializeField] private DialogueManager _dialogueManager;
        [SerializeField] private CamerasHandler _camerasHandler;
        [SerializeField] private InputReader _inputReader;
        
        public override void InstallBindings()
        {
            Container.Bind<DialogueManager>().FromInstance(_dialogueManager);
            Container.Bind<CamerasHandler>().FromInstance(_camerasHandler);
            Container.Bind<InputReader>().FromInstance(_inputReader);
        }
    }
}