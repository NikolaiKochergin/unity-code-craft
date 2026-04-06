using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.App
{
    public sealed class DebugLogRepository : IRepository
    {
        private readonly IRepository _origin;

        public DebugLogRepository(IRepository origin) =>
            _origin = origin;
        
        public async UniTask<(bool, int)> Save(JObject data, CancellationToken ct = default)
        {
            (bool success, int version) result = await _origin.Save(data, ct);
            if (result.success)
            {
                Debug.Log($"<color=orange>Saved</color> {data}");
                return result;
            }
            
            Debug.Log($"<color=red>Save failed</color> {data}");
            return result;
        }

        public async UniTask<(bool, JObject)> Load(int version, CancellationToken ct = default)
        {
            (bool success, JObject data) = await _origin.Load(version, ct);
            if (!success)
            {
                Debug.Log("<color=red>Loading failed!</color>");
                return (false, data);
            }
            
            Debug.Log($"<color=green>Loaded</color> {data}");
            return (true, data);
        }
    }
}