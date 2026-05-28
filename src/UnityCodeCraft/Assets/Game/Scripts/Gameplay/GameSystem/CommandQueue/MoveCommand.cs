using Modules.AI;
using UnityEngine;

namespace Game
{
    public sealed class MoveCommand : IUnitCommand
    {
        private readonly Vector3 _targetPosition;
        private readonly GameObject _targetObject;

        public MoveCommand(Vector3 targetPosition) => 
            _targetPosition = targetPosition;

        public MoveCommand(GameObject targetObject) => 
            _targetObject = targetObject;

        public void Start(Blackboard blackboard)
        {
            if (_targetObject)
            {
                blackboard.SetReferenceValue(BlackboardAPI.MovePoint, _targetObject);
            }
            else
            {
                GameObject basePoint = blackboard.GetValue(BlackboardAPI.BasePoint);
                basePoint.transform.position = _targetPosition;
                blackboard.SetReferenceValue(BlackboardAPI.MovePoint, basePoint);
            }
        }

        public void Stop(Blackboard blackboard)
        {
            if (_targetObject)
            {
                GameObject basePoint= blackboard.GetValue(BlackboardAPI.BasePoint);
                basePoint.transform.position = _targetObject.transform.position;
            }
            
            blackboard.DelValue(BlackboardAPI.MovePoint);
        }
    }
}