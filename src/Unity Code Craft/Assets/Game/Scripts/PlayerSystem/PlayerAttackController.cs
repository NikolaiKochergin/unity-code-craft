using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerAttackController : MonoBehaviour
    {
        [SerializeField]
        private BulletSystem _bulletWorld;

        [SerializeField]
        private Ship _player;

        private void OnEnable()
        {
            _player.Attack.OnFire += OnFire;
        }

        private void OnDisable()
        {
            _player.Attack.OnFire -= OnFire;
        }

        private void OnFire(AttackEvent attackEvent)
        {
            _bulletWorld.Spawn(
                attackEvent.FirePoint.position,
                attackEvent.FirePoint.up,
                attackEvent.Speed,
                attackEvent.Damage,
                TeamType.Player
            );
        }
    }
}