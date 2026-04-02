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

        public UniTask<bool> Save(JObject data, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public UniTask<(bool success, JObject data)> Load(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}