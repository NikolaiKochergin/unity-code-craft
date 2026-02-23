using UnityEngine;

namespace Game
{
    public class ShipAnimatorController : MonoBehaviour
    {
        [SerializeField] private Ship _ship;
        [SerializeField] private ShipAnimator _animator;

        private void Awake()
        {
            _ship.Mover.OnMoved += _animator.AnimateMovement;
            _ship.Attack.OnFire += _animator.AnimateFire;
            _ship.Health.OnChanged += _animator.AnimateDamage;
            _ship.Health.OnDied += _animator.AnimateDeath;
        }

        private void OnDestroy()
        {
            _ship.Mover.OnMoved -= _animator.AnimateMovement;
            _ship.Attack.OnFire -= _animator.AnimateFire;
            _ship.Health.OnChanged -= _animator.AnimateDamage;
            _ship.Health.OnDied -= _animator.AnimateDeath;
        }
    }
}