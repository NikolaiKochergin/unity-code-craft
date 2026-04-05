using Modules.Entities;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IComponentSerializer
    {
        void Serialize(Entity entity, JObject data);
        
        void Deserialize(Entity entity, JObject data);
    }

    public interface IComponentSerializer<in TComponent, TData> : 
        IComponentSerializer where TComponent : MonoBehaviour
    {
        void IComponentSerializer.Serialize(Entity entity, JObject data)
        {
            if (entity.TryGetComponent(out TComponent component))
                data[typeof(TComponent).Name] = JToken.FromObject(Serialize(component));
        }

        void IComponentSerializer.Deserialize(Entity entity, JObject data)
        {
            if(entity.TryGetComponent(out TComponent component) && 
               data.TryGetValue(typeof(TComponent).Name, out JToken componentData))
                Deserialize(component, componentData.ToObject<TData>());
        }

        TData Serialize(TComponent component);

        void Deserialize(TComponent component, TData data);
    }
}