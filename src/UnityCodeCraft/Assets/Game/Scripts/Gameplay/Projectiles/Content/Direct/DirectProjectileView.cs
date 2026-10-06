using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class DirectProjectileView : ProjectileView
    {
        [SerializeField] private DirectProjectileConfig _config;
        
        public override void OnSpawn(
            in Projectile previous, 
            in Projectile current, 
            float alpha, 
            PlayerRef player, 
            NetworkRunner runner)
        {
            UpdatePosition(in current, player, runner);
            UpdateRotation(in current);
        }

        public override void OnRender(
            in Projectile previous, 
            in Projectile current, 
            float alpha, 
            PlayerRef player, 
            NetworkRunner runner)
        {
            UpdatePosition(in current, player, runner);
            UpdateRotation(in current);
        }

        public override void OnDespawn(in Projectile previous, PlayerRef player, NetworkRunner runner) { }

        private void UpdatePosition(in Projectile projectile, PlayerRef player, NetworkRunner runner) => 
            transform.position = _config.GetRenderPosition(in projectile, player, runner);
        
        private void UpdateRotation(in Projectile projectile) => 
            transform.rotation = Quaternion.LookRotation(projectile.Direction);
    }
}