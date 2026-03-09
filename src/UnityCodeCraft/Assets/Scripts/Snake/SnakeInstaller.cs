using System;
using Modules;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public sealed class SnakeInstaller : Installer
    {
        [SerializeField] private Snake _snake;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ISnake>()
                .FromInstance(_snake)
                .AsSingle();
            
            Container
                .BindInterfacesTo<InputController>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<SnakeObserver>()
                .AsSingle();
        }
    }
}