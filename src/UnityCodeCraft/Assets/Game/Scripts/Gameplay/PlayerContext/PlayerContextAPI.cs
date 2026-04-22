using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

public static class PlayerContextAPI
{ 
    public static ValueKey<IPlayerContext, IReactiveVariable<int>> Health = new(nameof(Health));
    public static ValueKey<IPlayerContext, IValue<Animator>> Animator = new(nameof(Animator));
    public static ValueKey<IPlayerContext, TagKey> DamageTag = new(nameof(DamageTag));
}
