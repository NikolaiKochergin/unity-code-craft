using UnityEngine;

namespace Game
{
    public class MoveAbility : MonoBehaviour
    {
        [SerializeField] private MoveRequestComponent _moveRequest;
        [SerializeField] private MoveTransformComponent _moveComponent;
        
        public bool IsMoving => _moveRequest.IsMoving;

        private void Awake() => 
            _moveRequest.SetAction(_moveComponent.Move);

        public void Move(Vector2 direction) => 
            _moveRequest.Move(direction);
    }
}