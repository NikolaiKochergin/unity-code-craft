using System;
using UnityEngine;

namespace Game
{
    public interface IMover
    {
        event Action<Vector3, float> OnMoved;
        void MoveStep(Vector2 direction);
        void Update(float deltaTime);
    }
}