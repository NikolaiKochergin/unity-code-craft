using UnityEngine;

namespace Game
{
    public sealed class CircleTargetDetector : TargetDetector
    {
        [SerializeField] private float _radius = 1.5f;
        
        protected override int Cast(ContactFilter2D filter, Collider2D[] results) =>
            Physics2D.OverlapCircle(Origin, _radius, filter, results);

#if UNITY_EDITOR
        [SerializeField] private Color _gizmosColor;

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = _gizmosColor;
            UnityEditor.Handles.DrawWireDisc(Origin, Vector3.forward, _radius);
        }
#endif
    }
}