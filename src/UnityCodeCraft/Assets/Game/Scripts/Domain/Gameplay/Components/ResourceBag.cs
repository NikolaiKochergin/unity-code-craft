using Game.Gameplay;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ResourceBag : MonoBehaviour, IComponentSerializer<ResourceBagData>
    {
        ///Variable
        [field: SerializeField]
        public ResourceType Type { get; set; }
        
        ///Variable
        [field: SerializeField]
        public int Current { get; set; }
        
        ///Const
        [field: SerializeField]
        public int Capacity { get; set; }

        public string Key => nameof(ResourceBag);
        
        public ResourceBagData Serialize() =>
            new()
            {
                Type = Type,
                Current = Current,
            };

        public void Deserialize(ResourceBagData data)
        {
            Type = data.Type;
            Current = data.Current;
        }
    }

    public struct ResourceBagData
    {
        public ResourceType Type;
        public int Current;
    }
}