using Game.Gameplay;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class DestinationPoint : MonoBehaviour, IComponentSerializer<SerializedVector3>
    {
        ///Variable
        [field: SerializeField]
        public Vector3 Value { get; set; }

        public string Key => nameof(DestinationPoint);
        
        public SerializedVector3 Serialize() => Value;

        public void Deserialize(SerializedVector3 data) => Value = data;
    }
}