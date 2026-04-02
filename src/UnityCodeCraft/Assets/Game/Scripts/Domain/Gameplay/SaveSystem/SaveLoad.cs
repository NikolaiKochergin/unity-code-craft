using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Domain.App;
using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public class SaveLoad
    {
        private const string Versions = "versions";
        
        private readonly IRepository _gameRepository;
        private readonly ISaveSerializer[] _serializers;
        
        private JObject _gameData = new();

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
            
            JArray versions = _gameData[Versions] as JArray ?? new JArray();
            versions.Add(data);
            
            _gameRepository.Save(_gameData)
                .ContinueWith(success => callback?.Invoke(success, versions.Count))
                .Forget(e =>
                {
                    UnityEngine.Debug.LogException(e);
                    callback?.Invoke(false, -1);
                });
        }

        public void Load(int version, Action<bool, int> callback) =>
            _gameRepository.Load()
                .ContinueWith(result =>
                {
                    _gameData = result.data;
                    
                    if (!result.data.TryGetValue(Versions, out JToken token)) 
                        return;
                    
                    JArray versions = token as JArray;
                    if (versions == null || version < 0 || version >= versions.Count)
                    {
                        callback?.Invoke(false, version);
                        return;
                    }
                    
                    JObject versionData = versions[version] as JObject;
                    
                    if (versionData == null)
                    {
                        callback?.Invoke(false, version);
                        return;
                    }
                        
                    foreach (ISaveSerializer serializer in _serializers)
                        if (versionData.TryGetValue(serializer.Key, out JToken serializerData))
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