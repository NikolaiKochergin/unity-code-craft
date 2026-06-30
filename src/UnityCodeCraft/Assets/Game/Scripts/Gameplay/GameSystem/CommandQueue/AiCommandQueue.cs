using System;
using System.Collections.Generic;
using Modules.AI;
using Sirenix.OdinInspector;

namespace Game
{
    [Serializable]
    public sealed class AiCommandQueue
    {
        [ShowInInspector, HideInEditorMode]
        private readonly Queue<IAiCommand> _commandQueue = new();
        private readonly Blackboard _blackboard;

        public AiCommandQueue(Blackboard blackboard) => 
            _blackboard = blackboard;
        
        [ShowInInspector, HideInEditorMode]
        public IAiCommand CurrentCommand { get; private set; }
        public bool IsEmpty => _commandQueue.Count == 0;

        public void Add(IAiCommand command) => 
            _commandQueue.Enqueue(command);

        public bool TryStartNext()
        {
            StopCurrent();

            if (!_commandQueue.TryDequeue(out IAiCommand command)) 
                return false;
            
            StartCurrent(command);
            return true;
        }

        public AiCommandQueue Reset()
        {
            foreach (IAiCommand command in _commandQueue) 
                command.Dispose(_blackboard);
            
            _commandQueue.Clear();
            CurrentCommand?.Dispose(_blackboard);
            CurrentCommand = null;
            return this;
        }
        
        private void StartCurrent(IAiCommand command)
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