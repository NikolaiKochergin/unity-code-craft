    using SampleGame;
    using Unity.Entities;
    using Unity.Mathematics;
    using Unity.Rendering;
    using UnityEngine;

    namespace Game
    {
        public sealed partial class MeshTeamColorRenderSystem : SystemBase
        {
            private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
            
            private ComponentLookup<Team> _teamLookup;
            private TeamViewConfig _catalog;

            protected override void OnCreate()
            {
                _teamLookup = SystemAPI.GetComponentLookup<Team>(isReadOnly: true);
                _catalog = Resources.Load<TeamViewConfig>(nameof(TeamViewConfig));
            }

            protected override void OnUpdate()
            {
                _teamLookup.Update(this);

                foreach ((
                        RefRO<ModelEntity> modelEntity, 
                        RefRW<URPMaterialPropertyBaseColor> baseColor) 
                    in SystemAPI.Query<
                        RefRO<ModelEntity>,
                        RefRW<URPMaterialPropertyBaseColor>>())
                {
                    RefRO<Team> team = _teamLookup.GetRefRO(modelEntity.ValueRO.Value);
                    TeamViewConfig.TeamInfo info = _catalog.GetTeam(team.ValueRO.Value);
                    Color color = info.Material.GetColor(BaseColor);
                    baseColor.ValueRW.Value = new float4(color.r, color.g, color.b, color.a);
                }
            }
        }
    }