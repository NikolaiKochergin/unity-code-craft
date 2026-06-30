using Modules.AI;

namespace Game
{
    public interface IAiCommand
    {
        void Unpack(Blackboard blackboard);
        void Dispose(Blackboard blackboard);
    }
}