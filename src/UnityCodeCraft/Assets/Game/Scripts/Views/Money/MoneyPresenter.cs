using Modules.Money;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class MoneyPresenter : MonoBehaviour
    {
        private const string Format = "{0:0}";
        
        [SerializeField] private MoneyView _moneyView;
        
        private MoneyStorage _storage;

        [Inject]
        public void Construct(MoneyStorage storage) => 
            _storage = storage;

        private void OnEnable()
        {
            _moneyView.Setup(_storage.Money, Format);
            _storage.OnMoneyEarned += _moneyView.Earn;
            _storage.OnMoneySpent += _moneyView.Spend;
        }

        private void OnDisable()
        {
            _storage.OnMoneyEarned -= _moneyView.Earn;
            _storage.OnMoneySpent -= _moneyView.Spend;
        }
    }
}