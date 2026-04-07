using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Repository
{
    public sealed class PlayerPrefsRepository : IRepository
    {
        private readonly string _prefsKey;

        public PlayerPrefsRepository(string prefsKey) =>
            _prefsKey = prefsKey;

        public UniTask<(bool, int)> Save(JObject data, CancellationToken ct = default)
        {
            if (data == null)
                return UniTask.FromResult((false, -1));

            string raw = data.ToString();
            PlayerPrefs.SetString(_prefsKey + data["version"], raw);
            return UniTask.FromResult((true, data["version"].Value<int>()));
        }

        public UniTask<(bool, JObject)> Load(int version, CancellationToken ct = default)
        {
            if (!PlayerPrefs.HasKey(_prefsKey + version))
                return UniTask.FromResult((false, (JObject) null));
            
            string raw = PlayerPrefs.GetString(_prefsKey + version);
            JObject data = null;
            try
            {
                data = JObject.Parse(raw);
                return UniTask.FromResult((true, data));
            }
            catch (Exception)
            {
                return UniTask.FromResult((false, data));
            }
        }
    }
}