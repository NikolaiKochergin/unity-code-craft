using Modules.AI;
using UnityEngine;

namespace Game
{
    public class DetectCollidersBehaviour : MonoBehaviour
    {
        [SerializeField] 
        private Blackboard _blackboard;
        
        [SerializeField] 
        private Transform _center;
        
        [SerializeField]
        private float _radius;
        
        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField] 
        private QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.Ignore;

        private void FixedUpdate()
        {
            Collider[] buffer = _blackboard.GetValue(BlackboardAPI.ColliderBuffer);
            
            int count = Physics.OverlapSphereNonAlloc(
                _center.position,
                _radius,
                buffer,
                _layerMask.value,
                _triggerInteraction);
            
            _blackboard.SetPrimitiveValue(
                BlackboardAPI.ColliderCount,
                count);
        }

        private void OnDrawGizmos()
        {
            if(_center == null)
                return;
            
            Color prevColor = Gizmos.color;
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_center.position, _radius);
            
            Gizmos.color = prevColor;
        }
    }
}