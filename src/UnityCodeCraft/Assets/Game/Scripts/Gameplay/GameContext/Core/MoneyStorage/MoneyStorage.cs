using System;
using Fusion;

namespace Game
{
    public sealed class MoneyStorage : NetworkBehaviour
    {
        public event Action<int> OnMoneyChanged;
        
        [Networked, OnChangedRender(nameof(InvokeMoneyChanged))]
        public int Money { get; private set; }
        
        public void EarnMoney(int amount) => Money += amount;
        public void SpendMoney(int amount) => Money -= amount;
        
        private void InvokeMoneyChanged()  => OnMoneyChanged?.Invoke(Money);
    }
}