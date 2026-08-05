using SampleGame;
using Unity.Entities;

namespace Game
{
    public struct IsEnemyPredicate : IEntityPredicate
    {
        private readonly Entity _self;
        private readonly TeamType _team;
        
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<CurrentHealth> _healthLookup;

        public IsEnemyPredicate(
            Entity self,
            TeamType team,
            ComponentLookup<Team> teamLookup,
            ComponentLookup<CurrentHealth> healthLookup
        )
        {
            _self = self;
            _team = team;
            _teamLookup = teamLookup;
            _healthLookup = healthLookup;
        }

        public bool Invoke(Entity entity) =>
            entity != _self &&
            _teamLookup.TryGetComponent(entity, out Team team) && team.Value != _team &&
            _healthLookup.TryGetComponent(entity, out CurrentHealth health) && health.IsAlive();
    }
}