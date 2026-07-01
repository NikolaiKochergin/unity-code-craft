using Modules.AI;

namespace Game
{
    public interface IAICommand
    {
        void Unpack(Blackboard blackboard);
        void Dispose(Blackboard blackboard);
    }
}