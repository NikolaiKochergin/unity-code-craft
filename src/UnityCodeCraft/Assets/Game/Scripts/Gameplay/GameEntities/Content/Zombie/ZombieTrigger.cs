using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class ZombieTrigger : MonoBehaviour
    {
        [SerializeField] private GameEntity[] _zombies;
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.TryGetComponent(out IGameEntity entity) && entity.HasTag(GameEntityAPI.CharacterTag))
                foreach (GameEntity zombie in _zombies)
                    zombie.GetValue(GameEntityAPI.Target).Value = entity;
        }

        private void OnTriggerExit(Collider col)
        {
            if(col.TryGetComponent(out IGameEntity entity) && entity.HasTag(GameEntityAPI.CharacterTag))
                foreach (GameEntity zombie in _zombies)
                    zombie.GetValue(GameEntityAPI.Target).Value = null;
        }
    }
}