using UnityEngine;

namespace Game
{
    public class TossRequestComponent : CooldownComponent
    {
        public interface IAction
        {
            public void Invoke();
        }
        
        public interface ICondition
        {
            bool Evaluate();
        }

        [SerializeField] private bool _tossRequired;
        
        private IAction _tossAction;
        private ICondition _tossCondition;

        public void SetAction(IAction action) => _tossAction = action;
        public void SetCondition(ICondition condition) => _tossCondition = condition;
        
        public void Toss() => _tossRequired = true;
        
        public void FixedUpdate()
        {
            if (_tossRequired && IsExpired && (_tossCondition == null || _tossCondition.Evaluate()))
            {
                _tossAction?.Invoke();
                Reset();
            }
            
            _tossRequired = false;
        }
    }
}