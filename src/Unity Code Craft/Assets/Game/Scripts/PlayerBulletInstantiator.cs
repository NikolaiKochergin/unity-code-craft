using UnityEngine;

namespace Game
{
    // +
    public sealed class FireController : MonoBehaviour
    {
        [SerializeField]
        private BulletSystem _bulletWorld;

        [SerializeField]
        private Ship _player;

        private void OnEnable()
        {
            _player.OnFire += this.OnFire;
        }

        private void OnDisable()
        {
            _player.OnFire -= this.OnFire;
        }

        private void OnFire(Ship _)
        {
            _bulletWorld.Spawn(
                _player.firePoint.position,
                _player.firePoint.up,
                _player.bulletSpeed,
                _player.bulletDamage,
                TeamType.Player
            );
        }
    }
}