using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Game.Repository
{
    public interface IRepository
    {
        UniTask<(bool success, int version)> Save(JObject data, CancellationToken ct = default);
        UniTask<(bool success, JObject data)> Load(int version, CancellationToken ct = default);
    }
}