using UnityEngine;

namespace Game
{
    public class Platform : MonoBehaviour,
        MoveRequestComponent.IAction
    {
        private WayPointRequestComponent _wayPointRequestComponent;
        private MoveTransformComponent _moveComponent;
        

        private void Awake()
        {
            _moveComponent = GetComponentInChildren<MoveTransformComponent>();
            _wayPointRequestComponent = GetComponentInChildren<WayPointRequestComponent>();
            
            _wayPointRequestComponent.SetAction(this);
        }

        void MoveRequestComponent.IAction.Invoke(Vector2 direction) => 
            _moveComponent.Move(direction);
    }
}
