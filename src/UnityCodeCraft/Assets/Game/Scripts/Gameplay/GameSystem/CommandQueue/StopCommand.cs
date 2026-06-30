using Modules.AI;
using UnityEngine;

namespace Game
{
    public class StopCommand : IAiCommand
    {
        public void Unpack(Blackboard blackboard)
        {
            blackboard.GetValue(BlackboardAPI.CommandQueue).Reset();
            Vector3 characterPosition = blackboard.GetValue(BlackboardAPI.Character).transform.position;
            blackboard.SetReferenceValue(BlackboardAPI.BasePoint, new PositionWayPoint(characterPosition));
        }

        public void Dispose(Blackboard blackboard) { }
    }
}