using System;
using Modules.Planets;
using R3;

namespace Game.Presenters
{
    public class PlanetStatsPresenter : IDisposable
    {
        private readonly Planet _planet;
        private readonly ReactiveProperty<int> _population;
        private readonly ReactiveProperty<int> _level;
        private readonly ReactiveProperty<int> _income;

        public PlanetStatsPresenter(Planet planet)
        {
            _planet = planet;
            _population = new ReactiveProperty<int>(planet.Population);
            _level = new ReactiveProperty<int>(planet.Level);
            _income = new ReactiveProperty<int>(planet.MinuteIncome);
            MaxLevel = planet.MaxLevel;
        }
        
        public string PopulationFormat => "Population: {0:0}";
        public string LevelFormat => "Level: {0:0}/{1:0}";
        public string IncomeFormat => "Income: {0:0} / sec";
        
        public int MaxLevel { get; }

        public ReadOnlyReactiveProperty<int> Population => _population;
        public ReadOnlyReactiveProperty<int> CurrentLevel => _level;
        public ReadOnlyReactiveProperty<int> Income => _income;

        public void Initialize()
        {
            UpdateStats();
            _planet.OnUnlocked += UpdateStats;
            
            _planet.OnPopulationChanged += OnPopulationChanged;
            _planet.OnUpgraded += OnUpgraded;
            _planet.OnIncomeChanged += OnIncomeChanged;
        }

        public void Dispose()
        {
            _planet.OnUnlocked -= UpdateStats;
            
            _planet.OnPopulationChanged -= OnPopulationChanged;
            _planet.OnUpgraded -= OnUpgraded;
            _planet.OnIncomeChanged -= OnIncomeChanged;
        }

        private void UpdateStats()
        {
            OnPopulationChanged(_planet.Population);
            OnUpgraded(_planet.Level);
            OnIncomeChanged(_planet.MinuteIncome);
        }

        private void OnPopulationChanged(int population) => 
            _population.Value = population;

        private void OnUpgraded(int level) => 
            _level.Value = level;

        private void OnIncomeChanged(int minuteIncome) => 
            _income.Value = minuteIncome / 60;
    }
}