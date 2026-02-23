using Modules.UI;
using UnityEngine;

namespace Game
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private HealthView _healthView;
        [SerializeField] private Ship _ship;

        private void OnEnable()
        {
            _ship.Health.OnChanged += OnHealthChanged;
            _ship.Health.OnDied += _gameOverView.Show;
        }

        private void OnDisable()
        {
            _ship.Health.OnChanged += OnHealthChanged;
            _ship.Health.OnDied += _gameOverView.Show;
        }
        
        private void OnHealthChanged(int currentHealth) => 
            _healthView.SetHealth(currentHealth, _ship.Health.Max);
    }
}