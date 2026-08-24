using SampleGame;
using Unity.Entities;

namespace Game
{
    public struct IsFriendPredicate : IEntityPredicate
    {
        private readonly Entity _self;
        private readonly TeamType _team;
        
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<CurrentHealth> _healthLookup;
        private ComponentLookup<MaxHealth> _maxHealthLookup;

        public IsFriendPredicate(
            Entity self,
            TeamType team,
            ComponentLookup<Team> teamLookup,
            ComponentLookup<CurrentHealth> healthLookup,
            ComponentLookup<MaxHealth> maxHealthLookup)
        {
            _maxHealthLookup = maxHealthLookup;
            _self = self;
            _team = team;
            _teamLookup = teamLookup;
            _healthLookup = healthLookup;
        }

        public bool Invoke(Entity entity) =>
            entity != _self &&
            _teamLookup.TryGetComponent(entity, out Team team) && team.Value == _team &&
            _healthLookup.TryGetComponent(entity, out CurrentHealth health) && health.IsAlive() &&
            _maxHealthLookup.TryGetComponent(entity, out MaxHealth maxHealth) && health.Value < maxHealth.Value;
    }
}