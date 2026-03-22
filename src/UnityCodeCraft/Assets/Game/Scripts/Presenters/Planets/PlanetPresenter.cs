using System;
using Modules.Planets;
using R3;
using UnityEngine;

namespace Game.Presenters
{
    public class PlanetPresenter : IDisposable
    {
        private readonly Planet _planet;
        private readonly ReactiveProperty<Sprite> _icon;
        private readonly ReactiveProperty<bool> _isUnlocked;
        private readonly ReactiveProperty<int> _price;

        public PlanetPresenter(Planet planet)
        {
            _planet = planet;
            
            Name = planet.Name;
            Income = new IncomePresenter(planet);
            
            _isUnlocked = new ReactiveProperty<bool>(_planet.IsUnlocked);
            _icon = new ReactiveProperty<Sprite>(_planet.GetIcon(_planet.IsUnlocked));
            _price = new ReactiveProperty<int>(_planet.Price);
        }

        public string PriceFormat => "{0:0}";
        public string Name { get; }
        public IncomePresenter Income { get; }

        public ReadOnlyReactiveProperty<Sprite> Icon => _icon;
        public ReadOnlyReactiveProperty<bool> IsUnlocked => _isUnlocked;
        public ReadOnlyReactiveProperty<int> Price => _price;

        public void Initialize()
        {
            _planet.OnUnlocked += OnUnlocked;
            _planet.OnUpgraded += OnUpgraded;
            Income.Initialize();
        }

        public void Dispose()
        {
            _planet.OnUnlocked -= OnUnlocked;
            _planet.OnUpgraded -= OnUpgraded;
            Income.Dispose();
        }

        private void OnUpgraded(int level) => 
            _price.Value = _planet.Price;
        
        private void OnUnlocked()
        {
            _isUnlocked.Value = true;
            _icon.Value = _planet.GetIcon(true);
            _price.Value = _planet.Price;
        }

        public void OnClick()
        {
            if (!_planet.IsUnlocked)
                _planet.Unlock();
        }

        public void OnHold()
        {
            Debug.Log("<color=orange>OnHold");
        }
    }
}