using Modules.AI;
using UnityEngine;

namespace Game
{
    public class StopCommand : IAICommand
    {
        public void Unpack(Blackboard blackboard)
        {
            blackboard.GetValue(BlackboardAPI.CommandQueue).Reset();
            Vector3 characterPosition = blackboard.GetValue(BlackboardAPI.Character).transform.position;
            blackboard.SetReferenceValue(BlackboardAPI.BasePoint, new PositionPoint(characterPosition));
        }

        public void Dispose(Blackboard blackboard) { }
    }
}