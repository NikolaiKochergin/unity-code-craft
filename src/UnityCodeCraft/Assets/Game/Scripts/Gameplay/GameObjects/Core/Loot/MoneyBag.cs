using Fusion;

namespace Game
{
    public sealed class MoneyBag : NetworkBehaviour
    {
        [Networked]
        public int RewardAmount { get; set; }
    }
}