using System;
using Modules.AI;

namespace Game
{
    public interface IAICommand
    {
        Type NodeType { get; }
        void Unpack(Blackboard blackboard);
        void Dispose(Blackboard blackboard);
    }
}