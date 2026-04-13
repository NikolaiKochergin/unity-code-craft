using UnityEngine;

namespace Game
{
    public class PushComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 _pushForce = new(10, 2);
        
        public void Push(Transform target, Vector2 direction)
        {
            Vector2 force = Vector2.Scale(direction, _pushForce);
            
            if (target.TryGetComponent(out Rigidbody2D rigidBody))
                rigidBody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}