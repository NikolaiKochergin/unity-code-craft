using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Trap
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        
        private CollisionComponent _collisionComponent;

        private void Awake()
        {
            _collisionComponent = GetComponent<CollisionComponent>();

            _collisionComponent.OnEntered += OnEntered;
        }

        private void OnEntered(Collision2D collision)
        {
            if(!collision.otherCollider.TryGetComponent(out HealthComponent healthComponent))
                return;
            
            healthComponent.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
