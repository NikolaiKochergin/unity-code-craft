using Modules.AI;

namespace Game
{
    public sealed class MoveCommand : IAiCommand
    {
        private readonly IWayPoint _targetPoint;

        public MoveCommand(IWayPoint targetPoint) => 
            _targetPoint = targetPoint;

        public void Unpack(Blackboard blackboard)
        {
            if (_targetPoint != null) 
                blackboard.SetReferenceValue(BlackboardAPI.MovePoint, _targetPoint);
        }

        public void Dispose(Blackboard blackboard)
        {
            if (_targetPoint != null) 
                blackboard.SetReferenceValue(BlackboardAPI.BasePoint, new PositionWayPoint(_targetPoint.Position));
            
            blackboard.DelValue(BlackboardAPI.MovePoint);
        }
    }
}