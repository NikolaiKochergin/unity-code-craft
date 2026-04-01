using Game.Gameplay;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Team : MonoBehaviour, IComponentSerializer<int>
    {
        ///Variable
        [field: SerializeField]
        public TeamType Type { get; set; }

        public string Key => nameof(Team);
        
        public int Serialize() => (int)Type;

        public void Deserialize(int data) => Type = (TeamType)data;
    }
}