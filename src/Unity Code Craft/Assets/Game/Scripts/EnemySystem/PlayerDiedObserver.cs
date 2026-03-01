using UnityEngine;

namespace Game
{
    public class PlayerDiedObserver : MonoBehaviour
    {
        [SerializeField] private Ship _player;
        [SerializeField] private EnemySpawnCooldown _enemySpawnCooldown;

        private void OnEnable() => 
            _player.OnDied += _enemySpawnCooldown.Disable;

        private void OnDisable() => 
            _player.OnDied += _enemySpawnCooldown.Disable;
    }
}