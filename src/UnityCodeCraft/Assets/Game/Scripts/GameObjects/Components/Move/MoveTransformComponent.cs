using UnityEngine;

namespace Game
{
    public sealed class MoveTransformComponent : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 4.5f;

        public void Move(Vector2 direction, float deltaTime)
        {
            if (direction != Vector2.zero) 
                transform.Translate((Vector3) direction * (_speed * deltaTime));
        }
    }
}