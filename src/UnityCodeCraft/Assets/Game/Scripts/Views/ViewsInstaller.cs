using UnityEngine;
using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        [SerializeField] private MoneyParticleView moneyParticle;
        
        public override void InstallBindings()
        {
            Container
                .Bind<MoneyParticleView>()
                .FromInstance(moneyParticle)
                .AsSingle();
        }
    }
}