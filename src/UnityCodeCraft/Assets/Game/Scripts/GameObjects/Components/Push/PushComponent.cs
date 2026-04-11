using UnityEngine;

namespace Game
{
    public class PushComponent : DelayedAction
    {
        [SerializeField] private Transform _pushPoint;
        [SerializeField] private Vector2 _pushForce;
        [SerializeField] private ContactFilter2D _contactFilter;

        private readonly Collider2D[] _results = new Collider2D[1];
        
        public void Push()
        {
            if(Physics2D.OverlapCircle(_pushPoint.position, _pushForce.x, _contactFilter, _results) == 0)
                return;

            foreach (Collider2D result in _results)
                if(result.TryGetComponent(out Rigidbody2D rigidBody))
                    rigidBody.AddForce(Force(), ForceMode2D.Impulse);
            
        }

        private Vector2 Force() => 
            _pushPoint.right * _pushForce.x + _pushPoint.up * _pushForce.y;
    }
}