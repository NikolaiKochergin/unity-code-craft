using UnityEngine;

namespace Game
{
    public class Character : MonoBehaviour, IMoveComponent
    {
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private LookComponent _lookComponent;
        [SerializeField] private MoveAbility _moveAbility;
        [SerializeField] private JumpAbility _jumpAbility;
        [SerializeField] private ForceAbility _pushAbility;
        [SerializeField] private ForceAbility _tossAbility;

        private void Awake() => 
            _healthComponent.OnDied += OnDied;

        private void OnDestroy() => 
            _healthComponent.OnDied -= OnDied;

        public void Move(Vector2 direction)
        {
            if(_healthComponent.IsDied)
                return;
            
            _moveAbility.Move(direction);
            _lookComponent.Look(direction.x);
        }

        public void Push()
        {
            if (_healthComponent.IsAlive)
                _pushAbility.Apply();
        }

        public void Toss()
        {
            if (_healthComponent.IsAlive)
                _tossAbility.Apply();
        }

        public void Jump()
        {
            if (_healthComponent.IsAlive)
                _jumpAbility.Jump();
        }

        private void OnDied() => 
            GetComponent<Rigidbody2D>().simulated = false;
    }
}
