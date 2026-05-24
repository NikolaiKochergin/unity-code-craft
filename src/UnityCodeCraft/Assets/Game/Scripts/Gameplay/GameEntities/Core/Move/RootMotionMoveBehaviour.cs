using UnityEngine;

namespace Game.Gameplay
{
    public class RootMotionMoveBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private void OnAnimatorMove()
        {
            transform.parent.position += _animator.deltaPosition;
            transform.parent.rotation *= _animator.deltaRotation;
        }
    }
}