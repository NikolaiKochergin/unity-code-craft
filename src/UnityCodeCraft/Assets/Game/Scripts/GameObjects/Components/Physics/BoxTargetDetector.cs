using UnityEngine;

namespace Game
{
    public sealed class BoxTargetDetector : TargetDetector
    {
        [SerializeField] private Vector2 _size = new(1.3f, 1.3f);
        
        protected override int Cast(ContactFilter2D filter, Collider2D[] results) =>
            Physics2D.OverlapBox(Origin, _size, 0f, filter, results);

#if UNITY_EDITOR
        [SerializeField] private Color _gizmosColor;

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = _gizmosColor;
            UnityEditor.Handles.DrawWireCube(Origin, _size);
        }
#endif
    }
}