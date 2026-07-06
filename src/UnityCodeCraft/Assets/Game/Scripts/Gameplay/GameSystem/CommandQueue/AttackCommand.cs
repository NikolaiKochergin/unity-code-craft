using System;
using Modules.AI;

namespace Game
{
    public class AttackCommand : IAICommand
    {
        private readonly IPoint _target;

        public AttackCommand(IPoint target) => 
            _target = target;

        public Type NodeType => typeof(AttackNode);

        public void Unpack(Blackboard blackboard)
        {
            if (_target != null)
                blackboard.SetReferenceValue(BlackboardAPI.AttackTarget, _target);
            
            if(_target is TargetPoint targetPoint)
                blackboard.SetReferenceValue(BlackboardAPI.Enemy, targetPoint.GameObject);
        }

        public void Dispose(Blackboard blackboard)
        {
            if (_target != null)
                blackboard.SetReferenceValue(BlackboardAPI.BasePoint, new PositionPoint(_target.Position));
            
            blackboard.DelValue(BlackboardAPI.AttackTarget);
            blackboard.DelValue(BlackboardAPI.Enemy);
        }
    }
}