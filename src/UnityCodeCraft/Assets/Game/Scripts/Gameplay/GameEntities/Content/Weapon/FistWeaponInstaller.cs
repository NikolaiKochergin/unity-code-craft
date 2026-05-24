using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class FistWeaponInstaller : WeaponInstaller
    {
        [SerializeField] private TransformInstaller _transformInstaller;
        [SerializeField] private Const<int> _damage = 1;
        [SerializeField] private ReactiveVariable<TeamType> _team;
        [SerializeField, Min(0)] private Const<float> _fistRadius = 0.15f;
        [SerializeField] private Const<int> _targetLimit = 5;
        [SerializeField] private Collider[] _hitResults;
        
        public override void Install(IGameEntity entity)
        {
            base.Install(entity);
            _hitResults = new Collider[_targetLimit.Value];
            
            _transformInstaller.Install(entity);
            
            entity.AddValue(GameEntityAPI.Team, _team);

            entity.GetValue(GameEntityAPI.FireCommand)
                .AddAction(() => entity.DealDamageOverlap(_hitResults, _fistRadius,_damage));
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.softRed;
            Gizmos.DrawWireSphere(transform.position, _fistRadius);
        }
#endif
    }
}