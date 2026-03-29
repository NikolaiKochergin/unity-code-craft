using System;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Domain.App
{
    [Serializable]
    public class RepositoryInstaller : Installer
    {
        [SerializeField] private string _prefsKey = "GameData";

        public override void InstallBindings()
        {
            Container
                .Bind<IRepository>()
                .To<PlayerPrefsRepository>()
                .AsSingle()
                .WithArguments(_prefsKey);
        }
    }
}