using System;
using System.Collections.Generic;
using Modules.AI;
using Sirenix.OdinInspector;

namespace Game
{
    [Serializable]
    public sealed class AICommandQueue
    {
        [ShowInInspector, HideInEditorMode]
        private readonly Queue<IAICommand> _commandQueue = new();
        private readonly Blackboard _blackboard;

        public AICommandQueue(Blackboard blackboard) => 
            _blackboard = blackboard;
        
        [ShowInInspector, HideInEditorMode]
        public IAICommand CurrentCommand { get; private set; }
        public bool IsEmpty => _commandQueue.Count == 0;

        public void Add(IAICommand command) => 
            _commandQueue.Enqueue(command);

        public bool TryStartNext()
        {
            StopCurrent();

            if (!_commandQueue.TryDequeue(out IAICommand command)) 
                return false;
            
            StartCurrent(command);
            return true;
        }

        public AICommandQueue Reset()
        {
            foreach (IAICommand command in _commandQueue) 
                command.Dispose(_blackboard);
            
            _commandQueue.Clear();
            CurrentCommand?.Dispose(_blackboard);
            CurrentCommand = null;
            return this;
        }
        
        private void StartCurrent(IAICommand command)
        {
            CurrentCommand = command;
            CurrentCommand?.Unpack(_blackboard);
        }

        private void StopCurrent()
        {
            CurrentCommand?.Dispose(_blackboard);
            CurrentCommand = null;
        }
    }
}