using Fusion;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(
        fileName = "ProjectileConfig(Direct)",
        menuName = "Game/Direct Projectile Config" 
    )]
    public sealed class DirectProjectileConfig : ProjectileConfig
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _lifeTime = 5f;
        
        public override void OnSpawned(ref Projectile projectile, PlayerRef player, NetworkRunner runner) { }

        public override void OnSimulate(
            ref Projectile projectile, 
            PlayerRef player, 
            NetworkRunner runner, 
            out bool finished)
        {
            finished = false;
            
            float deltaTIme = runner.DeltaTime;
            float currentTime = (runner.Tick - projectile.StartTick) * deltaTIme;
            if (currentTime > _lifeTime)
            {
                finished = true;
                return;
            }

            float previousTime = Mathf.Max(0, currentTime - deltaTIme);
            Vector3 direction = projectile.Direction;
            Vector3 position = projectile.Position + direction * previousTime * _speed;

            bool wasHit = runner
                .GetPhysicsScene()
                .Raycast(position, direction, out RaycastHit hit, _speed * deltaTIme, _layerMask.value);

            if (wasHit)
            {
                NetworkId playerId = runner.GetPlayerObject(player).Id;
                DealDamage(hit.collider, playerId);
                finished = true;
            }
        }

        public override void OnGizmos(in Projectile projectile, PlayerRef player, NetworkRunner runner) { }

        public Vector3 GetRenderPosition(in Projectile projectile, PlayerRef player, NetworkRunner runner)
        {
            float deltaTime = runner.DeltaTime;
            float spawnTime = projectile.StartTick * deltaTime;
            
            float renderTime = runner.LocalPlayer == player || runner.IsServer
                ? runner.LocalRenderTime + deltaTime
                : runner.RemoteRenderTime + deltaTime;
            float t = renderTime - spawnTime;
            return projectile.Position + projectile.Direction * t * _speed;
        }

        private bool DealDamage(Collider collider, NetworkId player)
        {
            NetworkObject target = collider.GetComponentInParent<NetworkObject>();
            if (target == null ||
                target.Id == player ||
                !target.TryGetBehaviour(out TakeDamageComponent takeDamageComponent))
                return false;
            
            takeDamageComponent.TakeDamage(new TakeDamageArgs(player, _damage));
            return true;
        }
    }
}