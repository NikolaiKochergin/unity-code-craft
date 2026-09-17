using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class PlayerContextInstaller : MonoInstaller
    {
        [SerializeField] private PlayerCharacterProvider _playerCharacterProvider;

        public override void InstallBindings()
        {
            Container.Bind<PlayerCharacterProvider>().FromInstance(_playerCharacterProvider).AsSingle();
        }
    }
}