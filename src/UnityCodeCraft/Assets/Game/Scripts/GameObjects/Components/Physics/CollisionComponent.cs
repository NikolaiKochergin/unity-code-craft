using System;
using UnityEngine;

namespace Game
{
    public sealed class CollisionComponent : MonoBehaviour
    {
        public event Action<Collision2D> OnEntered;
        public event Action<Collision2D> OnExited;

        private void OnCollisionEnter2D(Collision2D other) => OnEntered?.Invoke(other);

        private void OnCollisionExit2D(Collision2D other) => OnExited?.Invoke(other);
    }
}