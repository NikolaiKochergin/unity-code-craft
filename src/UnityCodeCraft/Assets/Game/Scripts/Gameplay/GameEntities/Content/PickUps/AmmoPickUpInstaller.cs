using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class AmmoPickUpInstaller : GameEntityInstaller
    {
        [SerializeField] private int _ammo = 10;
        [SerializeField] private InteractableInstaller _interactableInstaller;
        
        public override void Install(IGameEntity entity)
        {
            _interactableInstaller.Install(entity);

            entity.GetValue(GameEntityAPI.InteractCommand)
                .AddCondition(target => target.HasTag(GameEntityAPI.CharacterTag))
                .AddAction(target =>
                {
                    if (target.CollectAmmoWithWeapon(_ammo))
                        gameObject.SetActive(false);
                });
        }
    }
}