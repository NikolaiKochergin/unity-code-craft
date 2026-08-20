using SampleGame;

namespace Game
{
    public class UnitCardVewPresenter
    {
        private readonly UnitCardView _card;
        private readonly UnitCardConfig _config;

        public UnitCardVewPresenter(UnitCardView card, UnitCardConfig config)
        {
            _card = card;
            _config = config;
            Setup();
        }

        private void Setup()
        {
            _card.SetIcon(_config.Icon);
            _card.SetName(_config.Name);
        }

        public void Update()
        {
            
        }
    }
}