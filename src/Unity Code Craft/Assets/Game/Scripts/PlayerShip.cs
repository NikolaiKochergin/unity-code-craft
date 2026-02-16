using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerShipController : MonoBehaviour
    {
        [SerializeField] private Ship _ship;
        
        [SerializeField]
        private TransformBounds _playerArea;

        [SerializeField]
        private CameraShaker _cameraShaker;

        [Header("UI")]
        [SerializeField]
        private GameOverView _gameOverView;

        [SerializeField]
        private HealthView _healthView;

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

        private void OnHealthChanged(int currentHealth)
        {
            _healthView.SetHealth(currentHealth, _ship.Health.Max);
            _cameraShaker.Shake();
        }

        public void Update()
        {
            if(!_ship.Health.IsAlive)
                return;
            
            if (Input.GetKeyDown(KeyCode.Space))
                _ship.Fire();

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");
            
            _ship.Mover.MoveStep(new Vector2(dx, dy));
        }

        private void LateUpdate()
        {
            _ship.transform.position = _playerArea.ClampInBounds(this.transform.position);
        }
    }
}