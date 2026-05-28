using Modules.AI;

namespace Game
{
    public interface IUnitCommand
    {
        void Start(Blackboard blackboard);
        void Stop(Blackboard blackboard);
    }
}