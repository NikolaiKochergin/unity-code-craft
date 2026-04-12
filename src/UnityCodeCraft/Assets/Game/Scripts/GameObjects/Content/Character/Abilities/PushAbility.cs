using UnityEngine;

namespace Game
{
    public class PushAbility : MonoBehaviour, PushRequestComponent.IAction
    {
        private PushRequestComponent _pushRequest;
        private PushComponent _pushAction;

        private void Awake()
        {
            _pushRequest = GetComponent<PushRequestComponent>();
            _pushAction = GetComponent<PushComponent>();
            
            _pushRequest.SetAction(this);
        }

        void PushRequestComponent.IAction.Invoke() => _pushAction.Push();
    }
}