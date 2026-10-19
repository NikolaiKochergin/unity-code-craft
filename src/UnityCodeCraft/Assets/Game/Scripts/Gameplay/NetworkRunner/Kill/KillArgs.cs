using Fusion;

namespace Game
{
    public readonly struct KillArgs : INetworkStruct
    {
        public readonly PlayerRef Killer;
        public readonly NetworkId Victim;

        public KillArgs(PlayerRef killer, NetworkId victim)
        {
            Killer = killer;
            Victim = victim;
        }
    }
}