using SampleGame.Gameplay;

namespace Game.Gameplay
{
    public class CountdownComponentSerializer : IComponentSerializer<Countdown, float>
    {
        public float Serialize(Countdown component) => component.Current;

        public void Deserialize(Countdown component, float data) => component.Current = data;
    }
}