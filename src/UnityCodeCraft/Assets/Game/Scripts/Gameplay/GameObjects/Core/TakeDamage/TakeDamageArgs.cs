using Fusion;

namespace Game
{
    public readonly struct TakeDamageArgs : INetworkStruct
    {
        public readonly NetworkId Instigator;
        public readonly int Damage;

        public TakeDamageArgs(NetworkId instigator, int damage)
        {
            Instigator = instigator;
            Damage = damage;
        }
    }
}