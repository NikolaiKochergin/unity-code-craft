using System;
using Fusion;
using UnityEngine;

namespace Game
{
    public enum VfxType
    {
        Explode = 0,
        EnemyDie = 1,
    }

    [Serializable]
    public class VfxInfo
    {
        public VfxType Type;
        public ParticleSystem Prefab;
    }
    
    public sealed class VfxBus : NetworkBehaviour
    {
        [SerializeField] private VfxInfo[] _vfxCatalog;
        
        public void PlayVfx(VfxType type, Transform target)
        {
            if(!Runner.IsServer)
                return;
            
            if(target == null)
                return;
            
            RpcSpawnVfx(type, target.position, target.rotation);
        }

        [Rpc(
            RpcSources.StateAuthority,
            RpcTargets.All,
            InvokeLocal = true,
            TickAligned = false,
            Channel = RpcChannel.Reliable,
            HostMode = RpcHostMode.SourceIsServer)]
        private void RpcSpawnVfx(
            VfxType type,
            Vector3 position,
            Quaternion rotation)
        {
            if (!TryGetPrefab(type, out ParticleSystem prefab))
                return;

            Instantiate(prefab, position, rotation);
        }

        private bool TryGetPrefab(
            VfxType type,
            out ParticleSystem prefab)
        {
            foreach (VfxInfo info in _vfxCatalog)
            {
                if (info.Type != type || info.Prefab == null)
                    continue;

                prefab = info.Prefab;
                return true;
            }

            prefab = null;
            return false;
        }
    }
}