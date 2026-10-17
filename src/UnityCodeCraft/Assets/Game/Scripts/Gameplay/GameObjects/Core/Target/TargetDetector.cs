using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public sealed class TargetDetector : NetworkBehaviour
    {
        [SerializeField] private Transform _detectionPoint;
        [SerializeField] private float _detectionRadius = 0.5f;
        [SerializeField] private LayerMask _layerMask;

        private readonly List<NetworkObject> _targets = new();
        private readonly Collider[] _results = new Collider[10];
        
        [ShowInInspector]
        public IReadOnlyList<NetworkObject> Targets => _targets;
        
        public bool HasTarget => _targets.Count > 0;

        public bool TryGetNearestTarget(out NetworkObject nearestTarget)
        {
            nearestTarget = null;
            float nearestDistanceSqr = float.MaxValue;
            foreach (NetworkObject target in _targets)
            {
                float distanceSqr = (target.transform.position - _detectionPoint.position).sqrMagnitude;
                if (distanceSqr > nearestDistanceSqr)
                    continue;
                
                nearestDistanceSqr = distanceSqr;
                nearestTarget = target;
            }

            return nearestTarget;
        }

        public void Scan()
        {
            _targets.Clear();
            int count = Runner.GetPhysicsScene().OverlapSphere(
                _detectionPoint.position,
                _detectionRadius,
                _results,
                _layerMask,
                QueryTriggerInteraction.Ignore
            );
            
            for (int i = 0; i < count; i++)
            {
                Collider hit = _results[i];
                if(!hit)
                    continue;
                
                NetworkObject other = hit.GetComponentInParent<NetworkObject>();
                if (other && other.TryGetBehaviour(out HealthComponent health) && health.IsAlive)
                {
                    _targets.Add(other);
                    break;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_detectionPoint != null)
            {
                Gizmos.color = Color.darkOrange;
                Gizmos.DrawWireSphere(_detectionPoint.position, _detectionRadius);
            }
        }
    }
}