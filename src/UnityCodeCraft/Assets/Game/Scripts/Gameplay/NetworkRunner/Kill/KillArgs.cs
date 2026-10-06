using Fusion;

namespace Game
{
    public readonly struct KillArgs : INetworkStruct
    {
        public readonly NetworkId Killer;
        public readonly NetworkId Victim;

        public KillArgs(NetworkId killer, NetworkId victim)
        {
            Killer = killer;
            Victim = victim;
        }
    }
}