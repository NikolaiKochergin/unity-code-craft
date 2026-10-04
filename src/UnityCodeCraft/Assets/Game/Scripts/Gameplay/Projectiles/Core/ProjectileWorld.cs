using Fusion;
using UnityEngine;

namespace Game
{
    public class ProjectileWorld : NetworkBehaviour
    {
        private const int Capacity = 32;
        
        [SerializeField] private ProjectileConfig _projectileConfig;

        public int Length = Capacity;
        
        private ArrayReader<Projectile> _projectileReader =
            GetArrayReader<Projectile>(typeof(ProjectileWorld), nameof(_projectiles));
        
        [Networked, Capacity(Capacity)]
        private NetworkArray<Projectile> _projectiles { get; }

        public bool TrySpawn(Vector3 position, Quaternion rotation)
        {
            if(!FindFreeSlot(out int freeIndex))
                return false;

            NetworkRunner runner = Runner;
            Projectile projectile = new()
            {
                StartTick = runner.Tick,
                Position = position,
                Rotation = rotation,
            };
            
            _projectileConfig.OnSpawned(ref projectile, Object.InputAuthority, runner);
            _projectiles.Set(freeIndex, projectile);
            return true;
        }

        public void GetProjectileSnapshots(
            out NetworkArrayReadOnly<Projectile> previous,
            out NetworkArrayReadOnly<Projectile> current,
            out float alpha
        )
        {
            if (TryGetSnapshotsBuffers(
                    out NetworkBehaviourBuffer from,
                    out NetworkBehaviourBuffer to,
                    out alpha
                ))
            {
                previous = _projectileReader.Read(from);
                current = _projectileReader.Read(to);
            }
            else
            {
                previous = default;
                current = _projectiles;
            }
        }

        public override void FixedUpdateNetwork()
        {
            NetworkRunner runner = Runner;
            PlayerRef player = Object.InputAuthority;

            for (int i = 0; i < Capacity; i++)
            {
                ref Projectile projectile = ref _projectiles.GetRef(i);
                if(!projectile.IsAlive)
                    continue;
                
                _projectileConfig.OnSimulate(ref projectile, player, runner, out bool finished);

                if (finished)
                    projectile = default;
            }
        }

        private bool FindFreeSlot(out int index)
        {
            for (int i = 0; i < Capacity; i++)
            {
                Projectile projectile = _projectiles[i];
                if (!projectile.IsAlive)
                {
                    index = i;
                    return true;
                }
            }
            
            index = -1;
            return false;
        }
    }
}