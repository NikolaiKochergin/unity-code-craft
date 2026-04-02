using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.App
{
    public sealed class PlayerPrefsRepository : IRepository
    {
        private readonly string _prefsKey;

        public PlayerPrefsRepository(string prefsKey) =>
            _prefsKey = prefsKey;

        public UniTask<bool> Save(JObject data, CancellationToken ct = default)
        {
            if (data == null)
                return UniTask.FromResult(false);

            string raw = data.ToString();
            PlayerPrefs.SetString(_prefsKey, raw);
            return UniTask.FromResult(true);
        }

        public UniTask<(bool, JObject)> Load(CancellationToken ct = default)
        {
            if (!PlayerPrefs.HasKey(_prefsKey))
                return UniTask.FromResult((false, (JObject) null));
            
            string raw = PlayerPrefs.GetString(_prefsKey);
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