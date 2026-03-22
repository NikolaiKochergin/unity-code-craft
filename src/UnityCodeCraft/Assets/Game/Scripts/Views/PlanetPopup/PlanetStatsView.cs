using System;
using Game.Presenters;
using R3;
using UnityEngine;

namespace Game.Views
{
    public class PlanetStatsView : MonoBehaviour
    {
        [SerializeField] private Widget _population;
        [SerializeField] private Widget _level;
        [SerializeField] private Widget _income;
        
        private IDisposable _disposables;

        public void Initialize(PlanetStatsPresenter stats)
        {
            DisposableBuilder disposables = Disposable.CreateBuilder();

            stats.Population
                .Subscribe(population => _population.SetText(population, stats.PopulationFormat))
                .AddTo(ref disposables);
            
            stats.CurrentLevel
                .Subscribe(currentLevel => _level.SetText(currentLevel, stats.MaxLevel, stats.LevelFormat))
                .AddTo(ref disposables);
            
            stats.Income
                .Subscribe(income => _income.SetText(income, stats.IncomeFormat))
                .AddTo(ref disposables);

            _disposables = disposables.Build();
        }

        public void Dispose() => 
            _disposables?.Dispose();
    }
}