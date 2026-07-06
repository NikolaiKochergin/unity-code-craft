using System.Collections.Generic;
using Modules.AI;

namespace Game
{
    public static class ResetUseCase
    {
        public static void ResetQueue(Blackboard blackboard)
        {
            Queue<IAICommand> commandQueue = blackboard.GetValue(BlackboardAPI.CommandQueue);

            foreach (IAICommand command in commandQueue)
                command.Dispose(blackboard);

            commandQueue.Clear();

            if (blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out IAICommand currentCommand))
            {
                currentCommand.Dispose(blackboard);
                blackboard.DelValue(BlackboardAPI.CurrentCommand);
            }
        }
    }
}