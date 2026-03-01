using UnityEngine;

namespace Game
{
    public sealed class EnemyFactory : ComponentFactory<Enemy>
    {
        [SerializeField] private BulletSystem _bulletSystem;
        [SerializeField] private Ship _playerShip;
        
        protected override void Inject(Enemy enemy)
        {
            enemy.GetComponent<FireComponent>().Construct(_bulletSystem);
            enemy.SetTarget(_playerShip);
        }
    }
}