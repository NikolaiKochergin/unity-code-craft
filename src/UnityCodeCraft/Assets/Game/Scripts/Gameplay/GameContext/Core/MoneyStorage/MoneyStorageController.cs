using Fusion;
using Zenject;

namespace Game
{
    public sealed class MoneyStorageController : NetworkBehaviour
    {
        private MoneyStorage _moneyStorage;
        private KillNotificator _killNotificator;

        [Inject]
        public void Construct(MoneyStorage moneyStorage, KillNotificator killNotificator)
        {
            _killNotificator = killNotificator;
            _moneyStorage = moneyStorage;
        }

        public override void Spawned()
        {
            _killNotificator.OnKilled += OnKilled;
        }

        public override void Despawned(NetworkRunner _, bool __) => 
            _killNotificator.OnKilled -= OnKilled;

        private void OnKilled(KillArgs args)
        {
            if (Runner.FindObject(args.Killer)?.GetBehaviour<PlayerCharacterProvider>())
                _moneyStorage.EarnMoney(1);
        }
    }
}