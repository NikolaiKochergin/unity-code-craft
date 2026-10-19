using Fusion;

namespace Game
{
    public readonly struct TakeDamageArgs : INetworkStruct
    {
        public readonly PlayerRef Instigator;
        public readonly int Damage;

        public TakeDamageArgs(PlayerRef instigator, int damage)
        {
            Instigator = instigator;
            Damage = damage;
        }
    }
}