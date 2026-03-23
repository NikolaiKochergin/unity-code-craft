using UnityEngine;
using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        [SerializeField] private CoinParticleView _coinParticle;
        
        public override void InstallBindings()
        {
            Container
                .Bind<CoinParticleView>()
                .FromInstance(_coinParticle)
                .AsSingle();
        }
    }
}