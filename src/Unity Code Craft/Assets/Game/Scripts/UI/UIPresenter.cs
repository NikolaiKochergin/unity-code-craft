using Modules.UI;
using UnityEngine;

namespace Game
{
    public class UIPresenter : MonoBehaviour
    {
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private HealthView _healthView;
        [SerializeField] private Ship _playerShip;
        [SerializeField] private EnemyManager enemyManager;

        private void Start()
        {
            OnHealthChanged(_playerShip.HealthCurrent);
            OnDestroyedEnemiesChanged();
        }

        private void OnEnable()
        {
            _playerShip.OnHealthChanged += OnHealthChanged;
            _playerShip.OnDied += _gameOverView.Show;
            enemyManager.OnDestroyedEnemiesChanged += OnDestroyedEnemiesChanged;
        }

        private void OnDisable()
        {
            _playerShip.OnHealthChanged -= OnHealthChanged;
            _playerShip.OnDied -= _gameOverView.Show;
            enemyManager.OnDestroyedEnemiesChanged -= OnDestroyedEnemiesChanged;
        }

        private void OnHealthChanged(int currentHealth) => 
            _healthView.SetHealth(currentHealth, _playerShip.HealthMax);

        private void OnDestroyedEnemiesChanged() => 
            _scoreView.SetValue(enemyManager.DestroyedEnemies);
    }
}