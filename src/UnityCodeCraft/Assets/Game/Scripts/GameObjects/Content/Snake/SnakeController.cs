using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Snake
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private Snake _snake;
        [SerializeField] private CollisionComponent _collisionComponent;
        [SerializeField] private TriggerComponent _triggerComponent;

        private GameObject _target;
        
        private void Awake()
        {
            _collisionComponent.OnEntered += Attack;
            _triggerComponent.OnEntered += OnCharacterDetected;
            _triggerComponent.OnExited += OnCharacterLost;
        }

        private void OnDestroy()
        {
            _collisionComponent.OnEntered -= Attack;
            _triggerComponent.OnEntered -= OnCharacterDetected;
            _triggerComponent.OnExited -= OnCharacterLost;
        }

        private void OnCharacterDetected(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Character"))
                _target = col.gameObject;
        }

        private void OnCharacterLost(Collider2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Character"))
                _target = null;
        }

        private void Update()
        {
            if(!_target)
                return;
            
            Vector2 direction = (_target.transform.position - transform.position).normalized;
            _snake.Move(direction);
        }

        private void Attack(Collision2D col)
        {
            if(col.gameObject.layer == LayerMask.NameToLayer("Character"))
                _snake.Attack();
        }
    }
}