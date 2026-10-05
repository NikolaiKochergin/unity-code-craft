using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class TargetDetector : NetworkBehaviour
    {
        private static readonly List<LagCompensatedHit> HitsBuffer = new();
        
        [SerializeField] private Transform _detectionPoint;
        [SerializeField] private float _detectionRadius = 0.5f;
        [SerializeField] private LayerMask _layerMask;

        private readonly List<NetworkObject> _targets = new();
        
        public IReadOnlyList<NetworkObject> Targets => _targets;
        
        public bool HasTarget()
        {
            Scan();
            return _targets.Count > 0;
        }

        public void Scan()
        {
            _targets.Clear();
            int count = Runner.LagCompensation.OverlapSphere(
                _detectionPoint.position,
                _detectionRadius,
                Object.InputAuthority,
                HitsBuffer,
                _layerMask,
                HitOptions.IncludePhysX | HitOptions.SubtickAccuracy | HitOptions.IgnoreInputAuthority,
                clearHits: true,
                QueryTriggerInteraction.Ignore
            );
            
            for (int i = 0; i < count; i++)
            {
                LagCompensatedHit hit = HitsBuffer[i];
                Hitbox hitbox = hit.Hitbox;
                if(hitbox == null)
                    continue;
                
                NetworkObject other = hitbox.GetComponentInParent<NetworkObject>();
                if (other != null && other.TryGetBehaviour(out HealthComponent health) && health.IsAlive)
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