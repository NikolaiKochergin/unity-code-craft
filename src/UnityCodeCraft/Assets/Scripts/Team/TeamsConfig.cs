using System;
using UnityEngine;

namespace SampleGame
{
    [CreateAssetMenu(
        fileName = "TeamsConfig",
        menuName = "Game/New TeamsConfig"
    )]
    public sealed class TeamsConfig : ScriptableObject
    {
        [SerializeField]
        private TeamInfo[] _teams;

        public Color GetColor(TeamType type)
        {
            for (int i = 0, count = _teams.Length; i < count; i++)
            {
                TeamInfo info = _teams[i];
                if (type == info.type)
                    return info.color;
            }

            throw new Exception($"Team type is not found {type}");
        }
        
        public Material GetMaterial(TeamType type)
        {
            for (int i = 0, count = _teams.Length; i < count; i++)
            {
                TeamInfo info = _teams[i];
                if (type == info.type)
                    return info.material;
            }

            throw new Exception($"Team type is not found {type}");
        }

        [Serializable]
        public struct TeamInfo
        {
            [SerializeField]
            public TeamType type;

            [SerializeField]
            public Color color;

            [SerializeField]
            public Material material;
        }
    }
}