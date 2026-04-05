using SampleGame.Gameplay;

namespace Game.Gameplay
{
    public class HealthComponentSerializer : IComponentSerializer<Health, int>
    {
        public int Serialize(Health component) => component.Current;

        public void Deserialize(Health component, int data) => component.Current = data;
    }
}