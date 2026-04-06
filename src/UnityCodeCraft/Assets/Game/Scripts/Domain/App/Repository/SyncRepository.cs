using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.App
{
    public class SyncRepository : IRepository
    {
        private const string Version = "version";
        
        private readonly IRepository[] _repositories;
        
        private List<JObject> _saveDataList;

        public SyncRepository(params IRepository[] repositories) => 
            _repositories = repositories;

        public async UniTask<(bool, int)> Save(JObject data, CancellationToken ct = default)
        {
            int version = PlayerPrefs.GetInt(Version, 0) + 1;
            
            data[Version] = version;
            
            int count = _repositories.Length;
            if (count == 0)
                return (false, -1);
            
            UniTask<(bool, int)>[] tasks = new UniTask<(bool, int)>[count];
            for (int i = 0; i < count; i++) 
                tasks[i] = _repositories[i].Save(data, ct);
            
            (bool success, int version)[] result = await UniTask.WhenAll(tasks);
            
            PlayerPrefs.SetInt(Version, version);
            
            return result.Any(x => x.success) ? (true, version) : (false, -1);
        }

        public async UniTask<(bool success, JObject data)> Load(int version, CancellationToken ct = default)
        {
            int count = _repositories.Length;
            if (count == 0)
                return (false, null);
            
            UniTask<(bool, JObject)>[] tasks = new UniTask<(bool, JObject)>[count];
            for (int i = 0; i < count; i++) 
                tasks[i] = _repositories[i].Load(version, ct);
            
            (bool, JObject)[] results = await UniTask.WhenAll(tasks);
            
            JObject targetData = null;

            for (int i = 0; i < count; i++)
            {
                (bool success, JObject gameData) = results[i];
                if(!success || gameData == null)
                    continue;
                
                targetData = gameData;
                Debug.Log($"Target repository is {_repositories[i].GetType().Name}");
                break;
            }

            return targetData != null ? (true, targetData) : (false, null);
        }
    }
}