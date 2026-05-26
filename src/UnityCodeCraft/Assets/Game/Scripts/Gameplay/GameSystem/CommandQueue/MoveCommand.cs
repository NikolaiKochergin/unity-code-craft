using Modules.AI;
using UnityEngine;

namespace Game
{
    public sealed class MoveCommand : IUnitCommand
    {
        private readonly Blackboard _blackboard;
        private readonly Vector3 _targetPosition;

        public MoveCommand(Blackboard blackboard, Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
            _blackboard = blackboard;
        }

        public void Begin()
        {
            GameObject basePoint = _blackboard.GetValue(BlackboardAPI.BasePoint); 
            basePoint.transform.position = _targetPosition;
            _blackboard.SetReferenceValue(BlackboardAPI.MovePoint, basePoint);
        }
    }
}