using Modules.AI;
using UnityEngine;

namespace Game
{
    public class HoldPositionCommand : IUnitCommand
    {
        public void Start(Blackboard blackboard)
        {
            GameObject basePoint = blackboard.GetValue(BlackboardAPI.BasePoint);
            GameObject character = blackboard.GetValue(BlackboardAPI.Character);
            basePoint.transform.position = character.transform.position;
            blackboard.SetReferenceValue(BlackboardAPI.HoldPoint, basePoint);
        }

        public void Stop(Blackboard blackboard)
        {
            blackboard.DelValue(BlackboardAPI.HoldPoint);
        }
    }
}