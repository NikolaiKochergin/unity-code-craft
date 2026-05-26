using System;
using Modules.AI;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class HasTargetCondition : ICondition
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField] 
        [BlackboardValueKey(typeof(GameObject))]
        private string _targetKey;
        
        public bool Invoke() => 
            _blackboard.HasValue(_targetKey);
    }
}