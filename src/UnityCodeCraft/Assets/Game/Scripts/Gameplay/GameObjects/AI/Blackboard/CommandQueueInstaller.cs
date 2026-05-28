using System;
using Modules.AI;

namespace Game
{
    [Serializable]
    public sealed class CommandQueueInstaller : IBlackboardInstaller
    {
        public void Install(Blackboard blackboard) => 
            blackboard.AddReferenceValue(BlackboardAPI.CommandQueue, new UnitCommandQueue(blackboard));
    }
}