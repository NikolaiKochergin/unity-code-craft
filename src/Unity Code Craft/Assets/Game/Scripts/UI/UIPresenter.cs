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
        [SerializeField] private EnemyOrchestrator _enemyOrchestrator;

        private void Start()
        {
            OnHealthChanged(_playerShip.Health.Current);
            OnEnemyDied();
        }

        private void OnEnable()
        {
            _playerShip.Health.OnChanged += OnHealthChanged;
            _playerShip.Health.OnDied += _gameOverView.Show;
            _enemyOrchestrator.OnEnemyDied += OnEnemyDied;
        }

        private void OnDisable()
        {
            _playerShip.Health.OnChanged -= OnHealthChanged;
            _playerShip.Health.OnDied -= _gameOverView.Show;
            _enemyOrchestrator.OnEnemyDied -= OnEnemyDied;
        }

        private void OnHealthChanged(int currentHealth) => 
            _healthView.SetHealth(currentHealth, _playerShip.Health.Max);

        private void OnEnemyDied() => 
            _scoreView.SetValue(_enemyOrchestrator.DestroyedEnemies);
    }
}