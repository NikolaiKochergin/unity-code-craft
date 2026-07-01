using Modules.AI;

namespace Game
{
    public sealed class MoveCommand : IAICommand
    {
        private readonly IPoint _targetPoint;

        public MoveCommand(IPoint targetPoint) => 
            _targetPoint = targetPoint;

        public void Unpack(Blackboard blackboard)
        {
            if (_targetPoint != null) 
                blackboard.SetReferenceValue(BlackboardAPI.MovePoint, _targetPoint);
        }

        public void Dispose(Blackboard blackboard)
        {
            if (_targetPoint != null) 
                blackboard.SetReferenceValue(BlackboardAPI.BasePoint, new PositionPoint(_targetPoint.Position));
            
            blackboard.DelValue(BlackboardAPI.MovePoint);
        }
    }
}