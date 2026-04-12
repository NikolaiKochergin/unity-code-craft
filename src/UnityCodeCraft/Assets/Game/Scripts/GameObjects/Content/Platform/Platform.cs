using UnityEngine;

namespace Game
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private Transform[] _waypoints;
        [SerializeField, Min(0)] private float _reachDistance = 0.1f;
        
        private CollisionComponent _collisionComponent;
        
        private MoveTransformComponent _moveComponent;
        
        private int _waypointIndex;

        private void Awake()
        {
            _moveComponent = GetComponentInChildren<MoveTransformComponent>();

            _collisionComponent = GetComponentInChildren<CollisionComponent>();
            _collisionComponent.OnEntered += OnCollisionEntered;
            _collisionComponent.OnExited += OnCollisionExited;
        }

        private void OnCollisionEntered(Collision2D col) => 
            col.transform.SetParent(_moveComponent.transform);

        private void OnCollisionExited(Collision2D col) => 
            col.transform.SetParent(null);

        private void FixedUpdate()
        {
            Vector3 targetPosition = _waypoints[_waypointIndex].position;
            Vector3 currentPosition = _moveComponent.transform.position;
            
            Vector3 direction = (targetPosition - currentPosition).normalized;
            _moveComponent.Move(direction);

            if (Vector3.Distance(targetPosition, currentPosition) <= _reachDistance &&
                ++_waypointIndex >= _waypoints.Length)
                _waypointIndex = 0;
        }
    }
}
