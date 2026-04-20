using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Snake
{
    public class Snake : MonoBehaviour,
        IMoveComponent
    {
        [SerializeField] private MoveAbility _moveAbility;
        [SerializeField] private LookComponent _lookComponent;
        [SerializeField] private ForceAbility _pushAbility;
        
        private HealthComponent _healthComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            
            _healthComponent.OnDied += OnDied;
        }
        
        private void OnDestroy()
        {
            _healthComponent.OnDied -= OnDied;
        }
        
        public void Move(Vector2 direction)
        {
            if(_healthComponent.IsDied)
                return;
            
            _moveAbility.Move(direction);
            _lookComponent.Look(direction.x);
        }

        public void Attack()
        {
            if(_healthComponent.IsAlive)
                _pushAbility.Apply();
        }

        private void OnDied() => 
            GetComponent<Rigidbody2D>().simulated = false;
    }
}