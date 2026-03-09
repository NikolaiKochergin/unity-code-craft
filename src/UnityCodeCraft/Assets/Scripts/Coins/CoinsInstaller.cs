using System;
using Modules;
using UnityEngine;
using Zenject;

namespace Game
{
    [Serializable]
    public class CoinsInstaller : Installer
    {
        [SerializeField] private Transform _coinsContainer;
        [SerializeField] private Coin _coinPrefab;
        
        public override void InstallBindings()
        {
            Container
                .Bind<CoinManager>()
                .AsSingle();
            
            Container
                .Bind<WorldPoints>()
                .AsSingle();
            
            Container
                .BindMemoryPool<Coin, CoinsPool>()
                .FromComponentInNewPrefab(_coinPrefab)
                .UnderTransform(_coinsContainer)
                .AsSingle();
        }
    }
}