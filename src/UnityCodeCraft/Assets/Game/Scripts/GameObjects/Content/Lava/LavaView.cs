using UnityEngine;

namespace Game
{
    public sealed class LavaView : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        private TriggerComponent _triggerComponent;

        private void Awake() => _triggerComponent = GetComponentInParent<TriggerComponent>();

        private void OnEnable() => _triggerComponent.OnEntered += OnTrigger;

        private void OnDisable() => _triggerComponent.OnEntered -= OnTrigger;

        private void OnTrigger(Collider2D obj) => _audioSource.Play();
    }
}