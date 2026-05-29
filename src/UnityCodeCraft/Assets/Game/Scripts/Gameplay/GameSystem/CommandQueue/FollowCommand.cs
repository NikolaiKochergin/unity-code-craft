using Modules.AI;
using UnityEngine;

namespace Game
{
    public class FollowCommand : IUnitCommand
    {
        private readonly GameObject _target;

        public FollowCommand(GameObject target) => 
            _target = target;

        public void Start(Blackboard blackboard) => 
            blackboard.SetReferenceValue(BlackboardAPI.FollowPoint, _target);

        public void Stop(Blackboard blackboard) => 
            blackboard.DelValue(BlackboardAPI.FollowPoint);
    }
}