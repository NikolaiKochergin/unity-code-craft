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
        
        private int _currentVersion;

        public SaveLoad(IRepository gameRepository, ISaveSerializer[] serializers)
        {
            _serializers = serializers;
            _gameRepository = gameRepository;
        }
        
        public void Save(Action<bool, int> callback) =>
            SaveAsync()
                .ContinueWith(result => callback?.Invoke(result.success, result.version))
                .Forget(e =>
                {
                    UnityEngine.Debug.LogException(e);
                    callback?.Invoke(false, -1);
                });

        public void Load(int version, Action<bool, int> callback) =>
            LoadAsync(version)
                .ContinueWith(result => callback?.Invoke(result.success, result.version))
                .Forget(e =>
                {
                    UnityEngine.Debug.LogException(e);
                    callback?.Invoke(false, version);
                });

        private async UniTask<(bool success, int version)> SaveAsync()
        {
            JObject gameData = new();
            foreach (ISaveSerializer serializer in _serializers)
                gameData.Add(serializer.Key, serializer.Serialize());
            
            return await _gameRepository.Save(gameData);
        }

        private async UniTask<(bool success, int version)> LoadAsync(int version)
        {
            (bool success, int version, JObject data) result = await _gameRepository.Load(version);

            if (result.success)
                foreach (ISaveSerializer serializer in _serializers)
                    if (result.data.TryGetValue(serializer.Key, out JToken data))
                        serializer.Deserialize(data);
            
            return result.success ? (true, result.version) : (false, version);
        }
    }
}