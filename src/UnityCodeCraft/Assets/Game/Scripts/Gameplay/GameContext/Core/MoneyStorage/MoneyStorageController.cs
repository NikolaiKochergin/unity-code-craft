using Fusion;
using Zenject;

namespace Game
{
    public sealed class MoneyStorageController : NetworkBehaviour, IKillHandler
    {
        private MoneyStorage _moneyStorage;

        [Inject]
        public void Construct(MoneyStorage moneyStorage) => 
            _moneyStorage = moneyStorage;

        public void Handle(KillArgs args)
        {
            if(!args.Killer.IsRealPlayer)
                return;

            int? reward = Runner.FindObject(args.Victim)?.GetBehaviour<MoneyBag>()?.RewardAmount;
            
            if(reward.HasValue)
                _moneyStorage.EarnMoney(reward.Value);
        }
    }
}