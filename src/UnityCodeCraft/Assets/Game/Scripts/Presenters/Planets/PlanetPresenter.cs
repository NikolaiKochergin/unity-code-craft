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

        public PlanetPresenter(Planet planet, PlanetPopupPresenter planetPopup)
        {
            _planet = planet;
            _planetPopup = planetPopup;
            
            Name = planet.Name;
            Income = new IncomePresenter(planet);
            Stats = new PlanetStatsPresenter(planet);
            
            _isUnlocked = new ReactiveProperty<bool>(_planet.IsUnlocked);
            _isMaxLevel = new ReactiveProperty<bool>(_planet.IsMaxLevel);
            _icon = new ReactiveProperty<Sprite>(_planet.GetIcon(_planet.IsUnlocked));
            _price = new ReactiveProperty<int>(_planet.Price);
        }

        public string PriceFormat => "{0:0}";
        public string Name { get; }
        public IncomePresenter Income { get; }
        public PlanetStatsPresenter Stats { get; }

        public ReadOnlyReactiveProperty<Sprite> Icon => _icon;
        public ReadOnlyReactiveProperty<bool> IsUnlocked => _isUnlocked;
        public ReadOnlyReactiveProperty<bool> IsMaxLevel => _isMaxLevel;
        public ReadOnlyReactiveProperty<int> Price => _price;

        public void Initialize()
        {
            _planet.OnUnlocked += OnUnlocked;
            _planet.OnUpgraded += OnUpgraded;
            Income.Initialize();
            Stats.Initialize();
        }

        public void Dispose()
        {
            _planet.OnUnlocked -= OnUnlocked;
            _planet.OnUpgraded -= OnUpgraded;
            Income.Dispose();
            Stats.Dispose();
        }

        private void OnUpgraded(int level)
        {
            _price.Value = _planet.Price;
            _isMaxLevel.Value = _planet.IsMaxLevel;
        }

        private void OnUnlocked()
        {
            _isUnlocked.Value = true;
            _icon.Value = _planet.GetIcon(true);
            _price.Value = _planet.Price;
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
    }
}