using System;
using Modules;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class GameplayInstaller : Installer
    {
        [SerializeField] private int _maxLevel = 9;
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<Score>()
                .AsSingle();
            
            Container
                .Bind<GameCycle>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<Difficulty>()
                .AsSingle()
                .WithArguments(_maxLevel);
        }
    }
}