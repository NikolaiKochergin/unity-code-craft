using System.Collections.Generic;
using Modules.Entities;
using Modules.Extensions;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    //Don't modify
    [CreateAssetMenu(
        fileName = "GameplayInstaller",
        menuName = "Zenject/New Gameplay Installer"
    )]
    public sealed class GameplayInstaller : ScriptableObjectInstaller
    {
        [SerializeField]
        private EntityCatalog _catalog;
        
        [SerializeField]
        private SaveLoadInstaller _saveLoad;
        
        public override void InstallBindings()
        {
            this.Container.Bind<EntityWorld>().FromComponentInHierarchy().AsSingle();
            this.Container.Bind<EntityCatalog>().FromInstance(_catalog).AsSingle();
            
            this.Container.Bind<ISaveSerializer>().To<EntityWorldSerializer>().AsSingle();

            this.Container.Bind<IComponentSerializer>().To<CountdownComponentSerializer>().AsSingle();
            this.Container.Bind<IComponentSerializer>().To<DestinationPointComponentSerializer>().AsSingle();
            this.Container.Bind<IComponentSerializer>().To<HealthComponentSerializer>().AsSingle();
            this.Container.Bind<IComponentSerializer>().To<ProductionOrderComponentSerializer>().AsSingle();
            this.Container.Bind<IComponentSerializer>().To<ResourceBagComponentSerializer>().AsSingle();
            this.Container.Bind<IComponentSerializer>().To<TargetObjectComponentSerializer>().AsSingle();
            this.Container.Bind<IComponentSerializer>().To<TeamComponentSerializer>().AsSingle();
            
            this.Container.Install(_saveLoad);
        }
    }
}