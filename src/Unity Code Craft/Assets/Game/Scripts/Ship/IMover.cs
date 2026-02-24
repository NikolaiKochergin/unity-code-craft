using UnityEngine;

namespace Game
{
    public interface IMover
    {
        Vector2 Direction { get; }
        void SetDirection(Vector2 direction);
        void Update(float deltaTime);
    }
}