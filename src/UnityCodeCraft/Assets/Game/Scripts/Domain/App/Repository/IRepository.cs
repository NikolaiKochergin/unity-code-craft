using System;
using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain.App
{
    public interface IRepository
    {
        void Save(JObject data, Action<bool, int> callback);
        void Load(string version, Action<bool, int, JObject> callback);
    }
}