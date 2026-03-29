using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public interface ISaveSerializer
    {
        string Key { get; }
        
        JToken Serialize();
        
        void Deserialize(JToken data);
    }

    public interface ISaveSerializer<T> : ISaveSerializer
    {
        JToken ISaveSerializer.Serialize() => JToken.FromObject(Serialize());
        
        void ISaveSerializer.Deserialize(JToken data) => Deserialize(data.ToObject<T>());

        new T Serialize();

        void Deserialize(T data);
    }
}