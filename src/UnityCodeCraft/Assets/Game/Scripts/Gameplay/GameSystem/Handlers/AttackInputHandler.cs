using Game;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class AttackInputHandler : InputHandler
    {
        [SerializeField] 
        private CommandMarkerView _commandMarkerView;
        
        [SerializeField]
        private KeyCode _keyCode = KeyCode.A;

        [SerializeField]
        private GameObject _character;

        [SerializeField] 
        private Blackboard _blackboard;

        [SerializeField]
        private InputHandler _next;

        public override void Handle(ref InputContext context)
        {
            if (Input.GetKey(_keyCode) && context.leftClick)
            {
                if (context.point != null)
                {
                    // TODO: Attack Position

                    GameObject basePoint = _blackboard.GetValue(BlackboardAPI.BasePoint);
                    
                    basePoint.transform.position = context.point.Value;
                    
                    _blackboard.SetReferenceValue(BlackboardAPI.Enemy, basePoint);
                    
                    _commandMarkerView.ShowAttackMarker(context.point.Value);
                }
                else if (context.target != null && context.target != _character)
                {
                    // TODO: Attack Target
                    
                    _blackboard.SetReferenceValue(BlackboardAPI.Enemy, context.target);
                    
                    GameObject basePoint = _blackboard.GetValue(BlackboardAPI.BasePoint);
                    
                    basePoint.transform.position = context.target.transform.position;
                    
                    
                    _commandMarkerView.ShowAttackMarker(context.target.transform);
                }
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}