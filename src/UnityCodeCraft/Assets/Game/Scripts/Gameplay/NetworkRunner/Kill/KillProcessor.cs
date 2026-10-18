using System.Collections.Generic;
using Fusion;
using Zenject;

namespace Game
{
    interface IKillHandler
    {
        void Handle(KillArgs args);
    }
    
    public sealed class KillProcessor : SimulationBehaviour
    {
        [Inject] private IEnumerable<IKillHandler> _handlers;
        
        public void ProcessKill(NetworkId killer, NetworkId victim)
        {
            KillArgs args = new(killer, victim);
            ProcessKill(args);
        }

        public void ProcessKill(KillArgs args)
        {
            if(!Runner.IsServer)
                return;
            
            foreach (IKillHandler handler in _handlers) 
                handler.Handle(args);
        }
    }
}