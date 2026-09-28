using Fusion;

namespace Game
{
    public readonly struct KillArgs : INetworkStruct
    {
        public readonly PlayerRef Killer;
        public readonly PlayerRef Victim;

        public KillArgs(PlayerRef killer, PlayerRef victim)
        {
            Killer = killer;
            Victim = victim;
        }
    }
}