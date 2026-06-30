using Modules.AI;
using UnityEngine;

namespace Game
{
    public class FollowCommand : IAiCommand
    {
        private readonly GameObject _target;

        public FollowCommand(GameObject target) => 
            _target = target;

        public void Unpack(Blackboard blackboard) => 
            blackboard.SetReferenceValue(BlackboardAPI.FollowTarget, _target);

        public void Dispose(Blackboard blackboard) => 
            blackboard.DelValue(BlackboardAPI.FollowTarget);
    }
}