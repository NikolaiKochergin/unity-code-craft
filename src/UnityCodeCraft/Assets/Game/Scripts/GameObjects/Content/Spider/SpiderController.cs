using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Spider
{
    public class SpiderController : MonoBehaviour
    {
        [SerializeField] private Spider _spider;
        [SerializeField] private Transform[] _waypoints;
        [SerializeField, Min(0)] private float _reachDistance = 0.1f;
        
        private int _waypointIndex;

        private void FixedUpdate()
        {
            Vector3 targetPosition = _waypoints[_waypointIndex].position;
            Vector3 currentPosition = _spider.transform.position;
            
            Vector3 direction = (targetPosition - currentPosition).normalized;
            _spider.Move(direction);

            if (Vector3.Distance(targetPosition, currentPosition) <= _reachDistance &&
                ++_waypointIndex >= _waypoints.Length)
                _waypointIndex = 0;
        }
    }
}