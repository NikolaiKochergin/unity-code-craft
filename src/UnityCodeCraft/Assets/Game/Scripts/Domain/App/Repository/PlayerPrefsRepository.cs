using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.App
{
    public sealed class PlayerPrefsRepository : IRepository
    {
        private const string Version = "version";
        
        private readonly string _prefsKey;

        public PlayerPrefsRepository(string prefsKey) =>
            _prefsKey = prefsKey;

        public UniTask<(bool success, int version)> Save(JObject data, CancellationToken ct = default)
        {
            int version = -1;
            if(data == null)
                return UniTask.FromResult<(bool success, int version)>((false, version));
            
            if(data.TryGetValue(Version, out JToken token))
                version = token.Value<int>();
            
            string raw = data.ToString();
            PlayerPrefs.SetString(_prefsKey, raw);
            return UniTask.FromResult((true, version));
        }

        public UniTask<(bool success, int version, JObject data)> Load(int version, CancellationToken ct = default)
        {
            if (!PlayerPrefs.HasKey(_prefsKey))
                return UniTask.FromResult((false, version, (JObject) null));
            
            string raw = PlayerPrefs.GetString(_prefsKey);
            try
            {
                JObject data = JObject.Parse(raw);
                
                if(data.TryGetValue(Version, out JToken token))
                    version = token.Value<int>();
                
                return UniTask.FromResult((true, version, data));
            }
            catch (Exception)
            {
                return UniTask.FromResult((false, version, (JObject) null));
            }
        }
    }
}