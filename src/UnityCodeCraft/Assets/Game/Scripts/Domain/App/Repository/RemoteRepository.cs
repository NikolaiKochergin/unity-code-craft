using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain.App
{
    public class RemoteRepository : IRepository
    {
        public UniTask<(bool success, int version)> Save(JObject data, CancellationToken ct = default)
        {
            
        }

        public UniTask<(bool success, int version, JObject data)> Load(int version, CancellationToken ct = default)
        {
            
        }
    }
}