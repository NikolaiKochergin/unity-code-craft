using UnityEngine;

namespace Game
{
    public class JumpRigidbodyComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField, Min(0)] private float _jumpForce;

        public void Jump() => 
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}