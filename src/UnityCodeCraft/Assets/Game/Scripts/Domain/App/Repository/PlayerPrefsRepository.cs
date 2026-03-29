using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.App
{
    public sealed class PlayerPrefsRepository : IRepository
    {
        private readonly string _prefsKey;
        private int _currentVersion;

        public PlayerPrefsRepository(string prefsKey) => 
            _prefsKey = prefsKey;

        public void Save(JObject data, Action<bool, int> callback)
        {
            if (data == null)
            {
                callback?.Invoke(false, _currentVersion);
                return;
            }

            data["version"] = (++_currentVersion).ToString();
            
            string raw = data.ToString();
            PlayerPrefs.SetString(_prefsKey, raw);
            callback?.Invoke(true, _currentVersion);
            
            
            Debug.Log($"<color=red> SERIALIZE </color> {raw}");
            
        }

        public void Load(string version, Action<bool, int, JObject> callback)
        {
            if (!PlayerPrefs.HasKey(_prefsKey))
            {
                callback?.Invoke(false, _currentVersion, null);
                return;
            }
            
            string raw = PlayerPrefs.GetString(_prefsKey);
            try
            {
                callback?.Invoke(true, _currentVersion, JObject.Parse(raw));
            }
            catch (Exception)
            {
                callback?.Invoke(false, _currentVersion, null);
            }
        }
    }
}