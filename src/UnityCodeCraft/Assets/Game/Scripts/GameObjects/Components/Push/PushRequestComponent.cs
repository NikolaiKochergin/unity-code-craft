using UnityEngine;

namespace Game
{
    public class PushRequestComponent : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke();
        }

        public interface ICondition
        {
            bool Evaluate();
        }

        [SerializeField] private bool _pushRequired;

        private IAction _pushAction;
        private ICondition _pushCondition;
        
        public void SetAction(IAction action) => _pushAction = action;
        public void SetCondition(ICondition condition) => _pushCondition = condition;
        
        public void Push() => _pushRequired = true;
        
        public void FixedUpdate()
        {
            if (_pushRequired && (_pushCondition == null || _pushCondition.Evaluate())) 
                _pushAction?.Invoke();
            
            _pushRequired = false;
        }
    }
}