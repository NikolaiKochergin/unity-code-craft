using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain.App
{
    public class SyncRepository : IRepository
    {
        private readonly IRepository[] _repositories;
        
        private List<JObject> _saveDataList;

        public SyncRepository(params IRepository[] repositories)
        {
            _repositories = repositories;
        }

        public async UniTask<(bool success, int version)> Save(JObject data, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async UniTask<(bool success, int version, JObject data)> Load(int version, CancellationToken ct = default)
        {
            int count = _repositories.Length;
            if(count == 0)
                return (false, -1, null);

            UniTask<(bool, int, JObject)>[] tasks = new UniTask<(bool, int, JObject)> [count];
            for (int i = 0; i < count; i++) 
                tasks[i] = _repositories[i].Load(version, ct);
            
            (bool, int, JObject)[] results = await UniTask.WhenAll(tasks);

            foreach ((bool, int, JObject) result in results)
            {
                
            }
            
            
            
        }
    }
}