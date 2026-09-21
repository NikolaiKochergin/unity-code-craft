using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class PlayerCameraInstaller : MonoInstaller
    {
        [SerializeField] private PlayerCameraView _playerCameraView;

        public override void InstallBindings()
        {
            Container.Bind<PlayerCameraView>().FromInstance(_playerCameraView).AsSingle();
        }
    }
}