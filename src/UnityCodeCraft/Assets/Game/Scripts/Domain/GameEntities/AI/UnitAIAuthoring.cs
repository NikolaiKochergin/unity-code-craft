using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class UnitAIAuthoring : MonoBehaviour
    {
        [SerializeField] private float _detectionRadius;
        [SerializeField] private float _detectionInterval;
        [SerializeField] private TeamType _detectionTeam;

        public class UnitAIBaker : Baker<UnitAIAuthoring>
        {
            public override void Bake(UnitAIAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With(new DetectionRadius { Value = authoring._detectionRadius })
                    .With(new DetectionCooldown{ Duration = authoring._detectionInterval })
                    .With(new DetectionTeam { TargetTeam = authoring._detectionTeam });
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
    }
}