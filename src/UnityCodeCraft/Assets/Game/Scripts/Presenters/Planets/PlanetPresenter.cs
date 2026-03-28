using System;
using Modules.Planets;
using R3;
using UnityEngine;

namespace Game.Presenters
{
    public class PlanetPresenter : IDisposable
    {
        private readonly Planet _planet;
        private readonly PlanetPopupPresenter _planetPopup;
        
        private readonly ReactiveProperty<Sprite> _icon;
        private readonly ReactiveProperty<bool> _isUnlocked;
        private readonly ReactiveProperty<bool> _isMaxLevel;
        private readonly ReactiveProperty<int> _price;
        
        private readonly ReactiveProperty<int> _population;
        private readonly ReactiveProperty<int> _level;
        private readonly ReactiveProperty<int> _minuteIncome;
        
        private readonly ReactiveProperty<bool> _isIncomeReady;
        private readonly ReactiveProperty<float> _incomeProgress;
        private readonly ReactiveProperty<TimeSpan> _remainTime;

        public PlanetPresenter(Planet planet, PlanetPopupPresenter planetPopup)
        {
            _planet = planet;
            _planetPopup = planetPopup;
            
            Name = planet.Name;
            MaxLevel = planet.MaxLevel;
            
            _isUnlocked = new ReactiveProperty<bool>(_planet.IsUnlocked);
            _isMaxLevel = new ReactiveProperty<bool>(_planet.IsMaxLevel);
            _icon = new ReactiveProperty<Sprite>(_planet.GetIcon(_planet.IsUnlocked));
            _price = new ReactiveProperty<int>(_planet.Price);
            
            _population = new ReactiveProperty<int>(planet.Population);
            _level = new ReactiveProperty<int>(planet.Level);
            _minuteIncome = new ReactiveProperty<int>(planet.MinuteIncome);
            
            _isIncomeReady = new ReactiveProperty<bool>(planet.IsIncomeReady);
            _incomeProgress = new ReactiveProperty<float>(planet.IncomeProgress);
            _remainTime = new ReactiveProperty<TimeSpan>();
        }
        
        public string Name { get; }
        public int MaxLevel { get; }
        public string PriceFormat => "{0:0}";
        public string PopulationFormat => "Population: {0:0}";
        public string LevelFormat => "Level: {0:0}/{1:0}";
        public string IncomeFormat => "Income: {0:0} / sec";
        public string RemainTimeFormat => "{0:00}m:{1:00}s";
        
        public ReadOnlyReactiveProperty<Sprite> Icon => _icon;
        public ReadOnlyReactiveProperty<bool> IsUnlocked => _isUnlocked;
        public ReadOnlyReactiveProperty<bool> IsMaxLevel => _isMaxLevel;
        public ReadOnlyReactiveProperty<int> Price => _price;
        
        public ReadOnlyReactiveProperty<int> Population => _population;
        public ReadOnlyReactiveProperty<int> CurrentLevel => _level;
        public ReadOnlyReactiveProperty<int> MinuteIncome => _minuteIncome;
        
        public ReadOnlyReactiveProperty<bool> IsIncomeReady => _isIncomeReady;
        public ReadOnlyReactiveProperty<float> IncomeProgress => _incomeProgress;
        public ReadOnlyReactiveProperty<TimeSpan> RemainTime => _remainTime;

        public void Initialize()
        {
            _planet.OnUnlocked += OnUnlocked;
            _planet.OnUpgraded += OnUpgraded;
            
            _planet.OnPopulationChanged += OnPopulationChanged;
            _planet.OnIncomeChanged += OnIncomeChanged;
            
            _planet.OnIncomeReady += OnIncomeReady;
            _planet.OnIncomeTimeChanged += OnIncomeTimeChanged;
        }

        public void Dispose()
        {
            _planet.OnUnlocked -= OnUnlocked;
            _planet.OnUpgraded -= OnUpgraded;
            
            _planet.OnPopulationChanged -= OnPopulationChanged;
            _planet.OnIncomeChanged -= OnIncomeChanged;
            
            _planet.OnIncomeReady -= OnIncomeReady;
            _planet.OnIncomeTimeChanged -= OnIncomeTimeChanged;
        }
        
        public void GatherIncome() => 
            _planet.GatherIncome();

        private void OnUnlocked()
        {
            _isUnlocked.Value = true;
            _icon.Value = _planet.GetIcon(true);
            
            OnUpgraded(_planet.Level);
        }

        private void OnUpgraded(int level)
        {
            _price.Value = _planet.Price;
            _level.Value = level;
            _isMaxLevel.Value = _planet.IsMaxLevel;
            
            OnPopulationChanged(_planet.Population);
            OnIncomeChanged(_planet.MinuteIncome);
        }

        public void Unlock()
        {
            if (!_planet.IsUnlocked)
                _planet.Unlock();
        }

        public void UnlockOrUpgrade() => 
            _planet.UnlockOrUpgrade();

        public void OnHold() => 
            _planetPopup.Show(this);
        
        private void OnPopulationChanged(int population) => 
            _population.Value = population;
        
        private void OnIncomeChanged(int minuteIncome) => 
            _minuteIncome.Value = minuteIncome / 60;
        
        private void OnIncomeReady(bool isReady)
        {
            _isIncomeReady.Value = isReady;
            _remainTime.Value = TimeSpan.Zero;
            _incomeProgress.Value = _planet.IncomeProgress;
        }

        private void OnIncomeTimeChanged(float value)
        {
            _remainTime.Value = TimeSpan.FromSeconds(value);
            _incomeProgress.Value = _planet.IncomeProgress;
        }
    }
}