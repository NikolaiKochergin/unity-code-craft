using Atomic.Elements;

namespace Game.UI
{
    public class StatPresenter : IGameUIInit, IGameUIDispose
    {
        private readonly StatView _statView;
        private readonly IReactiveVariable<int> _current;
        private readonly IValue<int> _max;
        private Subscription<int> _subscription;

        public StatPresenter(StatView statView, IReactiveVariable<int> current, IValue<int> max)
        {
            _max = max;
            _current = current;
            _statView = statView;
        }
        
        public void Init(IGameUI entity) => 
            _subscription = _current.Observe(OnChanged);

        public void Dispose(IGameUI entity) => 
            _subscription.Dispose();

        private void OnChanged(int current)
        {
            _statView.SetText(current.ToString());
            _statView.SetProgress(current / _max.Value);
        }
    }
}