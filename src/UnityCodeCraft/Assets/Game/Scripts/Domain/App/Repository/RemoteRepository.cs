using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Scripts.Domain.App
{
    public class RemoteRepository : IRepository
    {
        [ShowInInspector, ReadOnly]
        private readonly string _uri;

        [ShowInInspector, ReadOnly]
        private string _token;

        public RemoteRepository(string uri)
        {
            _uri = uri;
        }

        [Button]
        public async UniTask<bool> Register(string login, string password, CancellationToken ct = default)
        {
            var body = new JObject
            {
                ["login"] = login,
                ["password"] = password
            };

            byte[] bytes = Encoding.UTF8.GetBytes(body.ToString());

            var request = new UnityWebRequest($"{_uri}/register", "POST")
            {
                uploadHandler = new UploadHandlerRaw(bytes),
                downloadHandler = new DownloadHandlerBuffer()
            };

            request.SetRequestHeader("Content-Type", "application/json");

            try
            {
                await request.SendWebRequest().WithCancellation(ct);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Register cancelled");
                return false;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Register error: {request.error}");
                return false;
            }

            Debug.Log("Registered successfully");
            return true;
        }

        [Button]
        public async UniTask<bool> Login(string login, string password, CancellationToken ct = default)
        {
            var body = new JObject
            {
                ["login"] = login,
                ["password"] = password
            };

            byte[] bytes = Encoding.UTF8.GetBytes(body.ToString());

            var request = new UnityWebRequest($"{_uri}/login", "POST")
            {
                uploadHandler = new UploadHandlerRaw(bytes),
                downloadHandler = new DownloadHandlerBuffer()
            };

            request.SetRequestHeader("Content-Type", "application/json");

            try
            {
                await request.SendWebRequest().WithCancellation(ct);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Login cancelled");
                return false;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Login error: {request.error}");
                return false;
            }

            JObject response = JObject.Parse(request.downloadHandler.text);
            _token = response["token"]?.ToString();

            if (string.IsNullOrEmpty(_token))
                return false;

            Debug.Log($"Logged in: {_token}");
            return true;
        }

        public async UniTask<bool> Save(JObject gameData, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(_token))
                return false;

            var body = new JObject
            {
                ["data"] = gameData.ToString()
            };

            byte[] bytes = Encoding.UTF8.GetBytes(body.ToString());

            var request = new UnityWebRequest($"{_uri}/game/save", "POST")
            {
                uploadHandler = new UploadHandlerRaw(bytes),
                downloadHandler = new DownloadHandlerBuffer()
            };

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {_token}");
            
            try
            {
                await request.SendWebRequest().WithCancellation(ct);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Save cancelled");
                return false;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Save error: {request.error}");
                return false;
            }
            
            Debug.Log($"Save Completed {gameData}");
            return true;
        }

        public async UniTask<(bool, JObject)> Load(CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(_token))
                return (false, null);

            var request = UnityWebRequest.Get($"{_uri}/game/load");
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", $"Bearer {_token}");

            try
            {
                await request.SendWebRequest().WithCancellation(ct);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Load cancelled");
                return (false, null);
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Load error: {request.error}");
                return (false, null);
            }

            var response = JObject.Parse(request.downloadHandler.text);
            string jsonText = response["data"]?.ToString();

            if (string.IsNullOrEmpty(jsonText))
                return (false, null);

            JObject gameData = JObject.Parse(jsonText);
            return (true, gameData);
        }
    }
}