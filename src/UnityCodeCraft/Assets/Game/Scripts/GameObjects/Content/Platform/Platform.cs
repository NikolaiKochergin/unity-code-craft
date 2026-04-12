using UnityEngine;

namespace Game
{
    public class Platform : MonoBehaviour,
        MoveRequestComponent.IAction
    {
        private WayPointRequestComponent _wayPointRequestComponent;
        private MoveTransformComponent _moveComponent;
        
        private CollisionComponent _collisionComponent;

        private void Awake()
        {
            _moveComponent = GetComponentInChildren<MoveTransformComponent>();
            _wayPointRequestComponent = GetComponentInChildren<WayPointRequestComponent>();
            
            _wayPointRequestComponent.SetAction(this);

            _collisionComponent = GetComponentInChildren<CollisionComponent>();
            _collisionComponent.OnEntered += OnCollisionEntered;
            _collisionComponent.OnExited += OnCollisionExited;
        }

        private void OnCollisionEntered(Collision2D col) => 
            col.transform.SetParent(_moveComponent.transform);

        private void OnCollisionExited(Collision2D col) => 
            col.transform.SetParent(null);

        void MoveRequestComponent.IAction.Invoke(Vector2 direction) => 
            _moveComponent.Move(direction);
    }
}
