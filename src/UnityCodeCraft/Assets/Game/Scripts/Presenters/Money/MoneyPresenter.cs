using System;
using Modules.Money;
using R3;
using Zenject;

namespace Game.Presenters
{
    public class MoneyPresenter : IInitializable, IDisposable
    {
        private readonly MoneyStorage _storage;
        private readonly ReactiveProperty<int> _money;

        public MoneyPresenter(MoneyStorage moneyStorage)
        {
            _storage = moneyStorage;
            _money = new ReactiveProperty<int>(moneyStorage.Money);
        }

        public string Format => "{0:0}";
        
        public ReadOnlyReactiveProperty<int> Money => _money;
        
        public void Initialize() => 
            _storage.OnMoneyChanged += OnMoneyChanged;

        public void Dispose() => 
            _storage.OnMoneyChanged += OnMoneyChanged;

        private void OnMoneyChanged(int newValue, int prevValue) => 
            _money.Value = newValue;
    }
}
