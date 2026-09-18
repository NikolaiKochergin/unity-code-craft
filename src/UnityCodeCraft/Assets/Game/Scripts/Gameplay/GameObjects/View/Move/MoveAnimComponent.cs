using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class MoveAnimComponent : NetworkBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash(nameof(IsMoving));
        
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private Animator _animator;

        public override void Spawned()
        {
            _moveComponent.OnStateChanged += OnStateChanged;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _moveComponent.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged()
        {
            _animator.SetBool(IsMoving, _moveComponent.IsMoving);
        }
    }
}