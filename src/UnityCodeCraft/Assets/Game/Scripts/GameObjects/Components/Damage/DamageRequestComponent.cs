using UnityEngine;

namespace Game
{
    public class DamageRequestComponent : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke(HealthComponent health);
        }

        public interface ICondition
        {
            bool Evaluate();
        }
        
        [SerializeField] private bool _required;

        private IAction _action;
        private ICondition _condition;
        private GameObject _target;
        
        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;
        
        public void Damage(GameObject target)
        {
            _target = target;
            _required = true;
        }

        public void FixedUpdate()
        {
            if (_required && 
                _target.TryGetComponent(out HealthComponent health) && 
                (_condition == null || _condition.Evaluate())) 
                _action?.Invoke(health);
            
            _required = false;
        }
    }
}