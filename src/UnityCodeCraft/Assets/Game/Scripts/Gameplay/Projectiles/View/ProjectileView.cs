using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public abstract class ProjectileView : MonoBehaviour
    {
        [SerializeField] private Renderer[] _renderers;
        
        [Inject]
        private PlayerManager _playerManager;

        public abstract void OnSpawn(
            in Projectile previous,
            in Projectile current,
            float alpha,
            PlayerRef player,
            NetworkRunner runner
        );

        public abstract void OnRender(
            in Projectile previous,
            in Projectile current,
            float alpha,
            PlayerRef player,
            NetworkRunner runner
        );

        public abstract void OnDespawn(
            in Projectile previous,
            PlayerRef player,
            NetworkRunner runner
        );
    }
}