using System;
using App.Encryption;
using UnityEngine;
using Zenject;

namespace Game.Repository
{
    [Serializable]
    public class RepositoryInstaller : Installer
    {
        [SerializeField] private string _uri = "http://127.0.0.1:8888";
        [SerializeField] private string _prefsKey = "GameData";

        public override void InstallBindings()
        {
            Container
                .Bind<PlayerPrefsRepository>()
                .AsSingle()
                .WithArguments(_prefsKey);
            
            Container
                .Bind<RemoteRepository>()
                .AsSingle()
                .WithArguments(_uri);

            Container
                .Bind<IRepository>()
                .To<SyncRepository>()
                .FromMethod(CreateSyncRepository);

            Container
                .Decorate<IRepository>()
                .With<DebugLogRepository>();
            
            Container
                .Bind<IEncryptor>()
                .To<AesEncryptor>()
                .WithArguments("asldoo234aSl")
                .WhenInjectedInto<RemoteRepository>();
        }

        private SyncRepository CreateSyncRepository() => new(
            Container.Resolve<PlayerPrefsRepository>(),
            Container.Resolve<RemoteRepository>()
        );
    }
}