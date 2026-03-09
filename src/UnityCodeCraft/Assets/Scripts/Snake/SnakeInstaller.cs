using System;
using Modules;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public sealed class SnakeInstaller : Installer
    {
        [SerializeField] private SnakeInputConfig _inputConfig;
        [SerializeField] private Snake _snake;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ISnake>()
                .FromInstance(_snake)
                .AsSingle();
            
            Container
                .BindInterfacesTo<SnakeInputController>()
                .AsSingle()
                .WithArguments(_inputConfig);
            
            Container
                .BindInterfacesTo<SnakeSelfCollideObserver>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<SnakeWorldBoundsCollideObserver>()
                .AsSingle();
            
            Container
                .BindInterfacesTo<SnakeCoinPickObserver>()
                .AsSingle();
        }
    }
}