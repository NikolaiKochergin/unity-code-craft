using System;
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
            JObject gameData = new();
            foreach (ISaveSerializer serializer in _serializers)
                gameData.Add(serializer.Key, serializer.Serialize());
            
            _gameRepository.Save(gameData, callback);
        }

        public void Load(string version, Action<bool, int> callback)
        {
            _gameRepository.Load(version, OnLoaded);
            return;

            void OnLoaded(bool success, int dataVersion, JObject gameData)
            {
                if (success)
                {
                    foreach (ISaveSerializer serializer in _serializers)
                        if (gameData.TryGetValue(serializer.Key, out JToken data))
                            serializer.Deserialize(data);

                    callback?.Invoke(true, dataVersion);
                }
                else
                {
                    callback?.Invoke(false, dataVersion);
                }
            }
        }
    }
}