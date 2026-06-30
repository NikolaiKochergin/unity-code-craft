using Modules.AI;

namespace Game
{
    public class AttackCommand : IAiCommand
    {
        private readonly IWayPoint _target;

        public AttackCommand(IWayPoint target) => 
            _target = target;

        public void Unpack(Blackboard blackboard)
        {
            if (_target != null)
                blackboard.SetReferenceValue(BlackboardAPI.AttackTarget, _target);
        }

        public void Dispose(Blackboard blackboard)
        {
            if (_target != null)
                blackboard.SetReferenceValue(BlackboardAPI.BasePoint, new PositionWayPoint(_target.Position));
            
            blackboard.DelValue(BlackboardAPI.AttackTarget);
        }
    }
}