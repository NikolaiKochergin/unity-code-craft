using UnityEngine;

namespace Game
{
    public sealed class MoveTransformComponent : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _speed = 4.5f;

        public void Move(Vector2 direction)
        {
            if (direction != Vector2.zero) 
                _transform.Translate((Vector3) direction * _speed * Time.fixedDeltaTime);
        }
    }
}