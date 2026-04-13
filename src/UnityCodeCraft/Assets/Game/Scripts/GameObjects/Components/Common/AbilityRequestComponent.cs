using UnityEngine;

namespace Game
{
    public class AbilityRequestComponent : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke();
        }

        public interface ICondition
        {
            bool Evaluate();
        }

        [SerializeField] private bool _required;

        private IAction _action;
        private ICondition _condition;
        
        public void SetAction(IAction action) => _action = action;
        public void SetCondition(ICondition condition) => _condition = condition;
        
        public void Require() => _required = true;
        
        public void FixedUpdate()
        {
            if (_required && (_condition == null || _condition.Evaluate())) 
                _action?.Invoke();
            
            _required = false;
        }
    }
}