using Atomic.Entities;

namespace Game.Gameplay
{
    public struct KillArgs
    {
        public readonly TeamType Killer;
        public readonly TeamType Victim;

        public KillArgs(TeamType killer, TeamType victim)
        {
            Killer = killer;
            Victim = victim;
        }
    }
    
    public static class KillUseCase
    {
        public static void ProcessKill(this IGameContext gameContext, KillArgs args)
        {
            if(args.Killer == args.Victim)
                return;

            gameContext.GetValue(GameContextAPI.Score).Value++;
        }
    }
}