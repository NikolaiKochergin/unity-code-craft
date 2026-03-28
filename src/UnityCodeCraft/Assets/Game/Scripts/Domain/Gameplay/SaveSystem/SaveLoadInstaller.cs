using System;
using Zenject;

namespace Game.Gameplay
{
    [Serializable]
    public class SaveLoadInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container
                .Bind<SaveLoad>()
                .FromNew()
                .AsSingle();
        }
    }
}