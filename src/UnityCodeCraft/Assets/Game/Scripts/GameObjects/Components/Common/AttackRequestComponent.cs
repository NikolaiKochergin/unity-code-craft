using UnityEngine;

namespace Game
{
    public class AttackRequestComponent<TComponent> : MonoBehaviour where TComponent : Component
    {
        public interface ICondition
        {
            bool Evaluate();
        }

        public interface IAction
        {
            void Invoke(TComponent component);
        }

        [SerializeField] private bool _required;

        private IAction _action;
        private ICondition _condition;
        private GameObject _target;

        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;
        
        public void Require(GameObject target)
        {
            _target = target;
            _required = true;
        }

        public void FixedUpdate()
        {
            if (_required && 
                _target && 
                _target.TryGetComponent(out TComponent component) && 
                (_condition == null || _condition.Evaluate())) 
                _action?.Invoke(component);
            
            _required = false;
        }
    }
}