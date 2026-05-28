using Modules.AI;
using UnityEngine;

namespace Game
{
    public class StopCommand : IUnitCommand
    {
        public void Start(Blackboard blackboard)
        {
            blackboard.GetValue(BlackboardAPI.CommandQueue).Reset();
            GameObject basePoint = blackboard.GetValue(BlackboardAPI.BasePoint);
            GameObject character = blackboard.GetValue(BlackboardAPI.Character);
            basePoint.transform.position = character.transform.position;
        }

        public void Stop(Blackboard blackboard) { }
    }
}