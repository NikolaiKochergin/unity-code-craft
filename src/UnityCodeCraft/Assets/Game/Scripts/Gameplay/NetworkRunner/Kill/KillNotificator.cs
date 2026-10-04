using System;
using Fusion;

namespace Game
{
    public sealed class KillNotificator : SimulationBehaviour
    {
        public event Action<KillArgs> OnKilled;
        
        public void NotifyAboutKill(PlayerRef killer, PlayerRef victim)
        {
            KillArgs args = new(killer, victim);
            NotifyAboutKill(args);
        }

        public void NotifyAboutKill(KillArgs args)
        {
            if(!Runner.IsServer)
                return;

            if (args.Killer.IsRealPlayer && args.Victim.IsRealPlayer)
                RpcKill(Runner, args);
        }

        [Rpc(
            InvokeLocal = true,
            TickAligned = false,
            Channel = RpcChannel.Reliable,
            HostMode = RpcHostMode.SourceIsServer
        )]
        private static void RpcKill(NetworkRunner runner, KillArgs args)
        {
            KillNotificator notificator = runner.GetBehaviour<KillNotificator>();
            notificator.OnKilled?.Invoke(args);
        }
    }
}