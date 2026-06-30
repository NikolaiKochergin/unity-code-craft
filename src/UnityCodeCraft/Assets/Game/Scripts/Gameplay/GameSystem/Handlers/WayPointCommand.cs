using Modules.AI;

namespace Game
{
    public class WayPointCommand : IAiCommand
    {
        public void Unpack(Blackboard blackboard)
        {
            blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, 0);
        }

        public void Dispose(Blackboard blackboard)
        {
            blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, 0);
            blackboard.GetValue(BlackboardAPI.Waypoints).Clear();
        }
    }
}