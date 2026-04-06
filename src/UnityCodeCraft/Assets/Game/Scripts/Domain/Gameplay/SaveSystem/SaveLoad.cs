using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Domain.App;
using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public class SaveLoad
    {
        private readonly IRepository _gameRepository;
        private readonly ISaveSerializer[] _serializers;

        public SaveLoad(IRepository gameRepository, ISaveSerializer[] serializers)
        {
            _serializers = serializers;
            _gameRepository = gameRepository;
        }
        
        public void Save(Action<bool, int> callback)
        {
            JObject data = new();
            foreach (ISaveSerializer serializer in _serializers)
                data.Add(serializer.Key, serializer.Serialize());
            
            _gameRepository.Save(data)
                .ContinueWith(result => callback?.Invoke(result.success, result.version))
                .Forget(e =>
                {
                    UnityEngine.Debug.LogException(e);
                    callback?.Invoke(false, -1);
                });
        }

        public void Load(int version, Action<bool, int> callback) =>
            _gameRepository.Load(version)
                .ContinueWith(result =>
                {  
                    if(result.success)
                        foreach (ISaveSerializer serializer in _serializers)
                            if (result.data.TryGetValue(serializer.Key, out JToken serializerData))
                                serializer.Deserialize(serializerData);
                        
                    callback?.Invoke(result.success, version);
                })
                .Forget(e =>
                {
                    UnityEngine.Debug.LogException(e);
                    callback?.Invoke(false, version);
                });
    }
}