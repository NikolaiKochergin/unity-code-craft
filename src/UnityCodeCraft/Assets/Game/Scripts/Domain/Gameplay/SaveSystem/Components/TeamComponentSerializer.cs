using SampleGame.Common;
using SampleGame.Gameplay;

namespace Game.Gameplay
{
    public class TeamComponentSerializer : IComponentSerializer<Team, int>
    {
        public int Serialize(Team component) => (int)component.Type;

        public void Deserialize(Team component, int data) => component.Type = (TeamType)data;
    }
}