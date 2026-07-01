using Modules.AI;
using UnityEngine;

namespace Game
{
    public class HoldPositionCommand : IAICommand
    {
        public void Unpack(Blackboard blackboard)
        {
            GameObject character = blackboard.GetValue(BlackboardAPI.Character);
            blackboard.SetReferenceValue(BlackboardAPI.HoldPoint, new PositionPoint(character.transform.position));
        }

        public void Dispose(Blackboard blackboard)
        {
            blackboard.DelValue(BlackboardAPI.HoldPoint);
        }
    }
}