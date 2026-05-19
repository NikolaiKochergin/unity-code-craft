using System;
using Modules.AI;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class HasTargetPointCondition : ICondition
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField] 
        [BlackboardValueKey(typeof(Vector3))]
        private string _targetPointKey;
        
        public bool Invoke() => 
            _blackboard.HasValue(_targetPointKey);
    }
}