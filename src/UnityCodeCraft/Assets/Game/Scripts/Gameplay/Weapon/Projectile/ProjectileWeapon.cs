using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class ProjectileWeapon : Weapon
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _cooldown = 1;

        [Networked]
        private TickTimer CooldownTimestamp { get; set; }
        
        [Networked, UnityNonSerialized]
        private int FireCount { get; set; }

        private int _localFireCount;
        
        private PlayerManager _playerManager;
        
        public event Action OnFire;

        [Inject]
        public void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        
        public override void Spawned() => 
            _localFireCount = FireCount;

        public override bool CanFire() => 
            CooldownTimestamp.ExpiredOrNotRunning(Runner);

        public override void Fire()
        {
            if(!_playerManager.TryGetPlayerBehaviour(Object.InputAuthority, out ProjectileWorld world))
                return;

            world.TrySpawn(_firePoint.position, _firePoint.rotation);
            
            CooldownTimestamp = TickTimer.CreateFromSeconds(Runner, _cooldown);
        }

        public override void Render()
        {
            while (_localFireCount < FireCount)
            {
                OnFire?.Invoke();
                _localFireCount++;
            }
        }
    }
}