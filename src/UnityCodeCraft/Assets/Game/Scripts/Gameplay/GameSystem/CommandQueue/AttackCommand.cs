using Modules.AI;
using UnityEngine;

namespace Game
{
    public class AttackCommand : IAiCommand
    {
        private readonly Vector3 _targetPosition;
        private readonly GameObject _target;

        public AttackCommand(Vector3 targetPosition) => 
            _targetPosition = targetPosition;

        public AttackCommand(GameObject target) => 
            _target = target;

        public void Unpack(Blackboard blackboard)
        {
            if (_target)
            {
                blackboard.SetReferenceValue(BlackboardAPI.AttackTarget, _target);
            }
            else
            {
                // GameObject basePoint = blackboard.GetValue(BlackboardAPI.BasePoint);
                // basePoint.transform.position = _targetPosition;
                // blackboard.SetReferenceValue(BlackboardAPI.AttackTarget, basePoint);
            }
        }

        public void Dispose(Blackboard blackboard)
        {
            if (_target)
            {
                // GameObject basePoint= blackboard.GetValue(BlackboardAPI.BasePoint);
                // basePoint.transform.position = _target.transform.position;
            }
            
            blackboard.DelValue(BlackboardAPI.AttackTarget);
        }
    }
}