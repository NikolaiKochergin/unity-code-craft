using System;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Domain.App
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
        }

        private SyncRepository CreateSyncRepository() => new(
            Container.Resolve<PlayerPrefsRepository>(),
            Container.Resolve<RemoteRepository>()
        );
    }
}