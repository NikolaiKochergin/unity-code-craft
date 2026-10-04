using Fusion;
using TMPro;
using Zenject;

namespace Game
{
    public sealed class MoneyStoragePresenter : NetworkBehaviour
    {
        private TMP_Text _moneyAmountView;
        private MoneyStorage _moneyStorage;

        [Inject]
        public void Construct(
            [Inject(Id = Tags.Player)] TMP_Text moneyAmountView,
            MoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
            _moneyAmountView = moneyAmountView;
        }

        public override void Spawned()
        {
            UpdateMoneyAmountView(_moneyStorage.Money);
            _moneyStorage.OnMoneyChanged += UpdateMoneyAmountView;
        }

        public override void Despawned(NetworkRunner runner, bool hasState) => 
            _moneyStorage.OnMoneyChanged -= UpdateMoneyAmountView;

        private void UpdateMoneyAmountView(int amount) => 
            _moneyAmountView.SetText("{0}", amount);
    }
}