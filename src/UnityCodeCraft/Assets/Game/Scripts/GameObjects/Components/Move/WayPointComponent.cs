using UnityEngine;

namespace Game
{
    public class WayPointComponent : MonoBehaviour
    {
        [SerializeField] private MoveRequestComponent _moveRequestComponent;
        [SerializeField] private Transform[] _waypoints;
        [SerializeField, Min(0)] private float _reachDistance = 0.1f;
        
        private int _waypointIndex;

        private void FixedUpdate()
        {
            Vector3 targetPosition = _waypoints[_waypointIndex].position;
            Vector3 currentPosition = _moveRequestComponent.transform.position;
            
            Vector3 direction = (targetPosition - currentPosition).normalized;
            _moveRequestComponent.Move(direction);

            if (Vector3.Distance(targetPosition, currentPosition) <= _reachDistance &&
                ++_waypointIndex >= _waypoints.Length)
                _waypointIndex = 0;
        }
    }
}