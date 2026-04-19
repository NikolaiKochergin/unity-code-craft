using UnityEngine;

namespace Game
{
    public class JumpComponent : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _jumpForce;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        public void Jump() => 
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
}