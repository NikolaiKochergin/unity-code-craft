using UnityEngine;

namespace Game
{
    public class ForceComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 _force = new(10, 2);
        [SerializeField] private ForceMode2D _forceMode = ForceMode2D.Impulse;

        public void ApplyTo(Transform target, Vector2 direction)
        {
            Vector2 force = Vector2.Scale(direction, _force);
            
            if (target.TryGetComponent(out Rigidbody2D rigidbody)) 
                rigidbody.AddForce(force, _forceMode);
        }
    }
}