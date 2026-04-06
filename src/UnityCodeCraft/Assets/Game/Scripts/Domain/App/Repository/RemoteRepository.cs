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

        public RemoteRepository(string uri) => 
            _uri = uri;

        public async UniTask<(bool, int)> Save(JObject gameData, CancellationToken ct = default)
        {
            JObject body = new()
            {
                ["data"] = gameData.ToString()
            };
            
            byte[] bytes = Encoding.UTF8.GetBytes(body.ToString());

            var request = new UnityWebRequest($"{_uri}/save?version={gameData["version"]}", UnityWebRequest.kHttpVerbPUT)
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
                Debug.Log("Save cancelled");
                return (false, -1);
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Save error: {request.error}");
                return (false, -1);
            }
            
            Debug.Log($"Save Completed {gameData}");
            return (true, gameData["version"].Value<int>());
        }

        public async UniTask<(bool, JObject)> Load(int version, CancellationToken ct = default)
        {
            var request = UnityWebRequest.Get($"{_uri}/load?version={version}");
            request.downloadHandler = new DownloadHandlerBuffer();

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