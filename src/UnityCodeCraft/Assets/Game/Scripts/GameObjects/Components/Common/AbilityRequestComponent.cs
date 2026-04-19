using System;
using UnityEngine;

namespace Game
{
    public class AbilityRequestComponent : MonoBehaviour
    {
        [SerializeField] private bool _required;

        private Func<bool> _condition;
        private Action _action;
        
        public void SetCondition(Func<bool> condition) => _condition = condition;
        public void SetAction(Action action) => _action = action;
        
        public void Require() => _required = true;
        
        public void FixedUpdate()
        {
            if (_required && (_condition == null || _condition.Invoke())) 
                _action?.Invoke();
            
            _required = false;
        }
    }
}