using System;
using System.Collections.Generic;
using Modules.AI;
using Sirenix.OdinInspector;

namespace Game
{
    [Serializable]
    public sealed class UnitCommandQueue
    {
        [ShowInInspector, HideInEditorMode]
        private readonly Queue<IUnitCommand> _commandQueue = new();
        private readonly Blackboard _blackboard;

        public UnitCommandQueue(Blackboard blackboard) => 
            _blackboard = blackboard;
        
        public IUnitCommand CurrentCommand { get; private set; }
        public bool IsEmpty => _commandQueue.Count == 0;

        public void Add(IUnitCommand command) => 
            _commandQueue.Enqueue(command);

        public bool TryStartNext()
        {
            StopCurrent();

            if (!_commandQueue.TryDequeue(out IUnitCommand command)) 
                return false;
            
            StartCurrent(command);
            return true;
        }

        public UnitCommandQueue Reset()
        {
            StopCurrent();
            _commandQueue.Clear();
            return this;
        }
        
        private void StartCurrent(IUnitCommand command)
        {
            CurrentCommand = command;
            CurrentCommand?.Start(_blackboard);
        }

        private void StopCurrent()
        {
            CurrentCommand?.Stop(_blackboard);
            CurrentCommand = null;
        }
    }
}