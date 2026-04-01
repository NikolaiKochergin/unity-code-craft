using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public interface IComponentSerializer
    {
        string Key { get; }
        
        JToken Serialize();
        
        void Deserialize(JToken data);
    }

    public interface IComponentSerializer<TData> : IComponentSerializer
    {
        JToken IComponentSerializer.Serialize() => JToken.FromObject(Serialize());
        
        void IComponentSerializer.Deserialize(JToken data) => Deserialize(data.ToObject<TData>());

        new TData Serialize();

        void Deserialize(TData data);
    }
}