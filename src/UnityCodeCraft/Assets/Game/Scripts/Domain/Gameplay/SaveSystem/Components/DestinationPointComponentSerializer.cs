using SampleGame.Common;
using SampleGame.Gameplay;

namespace Game.Gameplay
{
    public class DestinationPointComponentSerializer : IComponentSerializer<DestinationPoint, SerializedVector3>
    {
        public SerializedVector3 Serialize(DestinationPoint component) => component.Value;

        public void Deserialize(DestinationPoint component, SerializedVector3 data) => component.Value = data;
    }
}