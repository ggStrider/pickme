using UnityEngine;
using Zenject;

using Cinemachine;
using Creature;
using Handlers;
using Player.Additional;
using Player.Main;

namespace Infrastructure
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private SprintSystem _sprintSystem;
        [SerializeField] private PlayerCameraFocusHandler _playerCameraFocusHandler;
        [SerializeField] private CinemachineVirtualCamera _playerCamera;

        public override void InstallBindings()
        {
            Container.Bind<IMovable>().FromInstance(_playerMovement);
            Container.Bind<SprintSystem>().FromInstance(_sprintSystem);
            Container.Bind<CinemachineVirtualCamera>().FromInstance(_playerCamera);
            Container.Bind<PlayerCameraFocusHandler>().FromInstance(_playerCameraFocusHandler);
        }
    }
}