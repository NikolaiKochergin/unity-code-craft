using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class MeleeWeapon : Weapon
    {
        private static readonly List<LagCompensatedHit> HitsBuffer = new();
        
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _fireRadius = 0.5f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _cooldown = 1;
        [SerializeField] private LayerMask _layerMask;
        
        [Networked]
        private TickTimer CooldownTimestamp { get; set; }
        
        public override bool CanFire() => 
            CooldownTimestamp.ExpiredOrNotRunning(Runner);

        public override void Fire()
        {
            int count = Runner.LagCompensation.OverlapSphere(
                _firePoint.position,
                _fireRadius,
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
                if (other != null && other.TryGetBehaviour(out HealthComponent health) && health.IsAlive
                    && other.TryGetBehaviour(out TakeDamageComponent takeDamageComponent))
                {
                    takeDamageComponent.TakeDamage(new TakeDamageArgs(Object.InputAuthority, _damage));
                    break;
                }
                
                CooldownTimestamp = TickTimer.CreateFromSeconds(Runner, _cooldown);
            }
        }
    }
}