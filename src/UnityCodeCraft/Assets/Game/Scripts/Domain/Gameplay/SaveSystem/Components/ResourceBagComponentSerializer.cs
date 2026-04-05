using SampleGame.Common;
using SampleGame.Gameplay;

namespace Game.Gameplay
{
    public class ResourceBagComponentSerializer : IComponentSerializer<ResourceBag, ResourceBagData>
    {
        public ResourceBagData Serialize(ResourceBag component) =>
            new()
            {
                Type = component.Type,
                Current = component.Current,
            };

        public void Deserialize(ResourceBag component, ResourceBagData data)
        {
            component.Type = data.Type;
            component.Current = data.Current;
        }
    }
    
    public struct ResourceBagData
    {
        public ResourceType Type;
        public int Current;
    }
}