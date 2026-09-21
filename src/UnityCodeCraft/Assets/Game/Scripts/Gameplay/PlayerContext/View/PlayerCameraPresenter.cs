using Fusion;
using Zenject;

namespace Game
{
    public sealed class PlayerCameraPresenter : NetworkBehaviour
    {
        private PlayerCharacterProvider _characterProvider;
        private PlayerCameraView _camera;
        
        [Inject]
        public void Construct(PlayerCharacterProvider characterProvider, PlayerCameraView camera)
        {
            _camera = camera;
            _characterProvider = characterProvider;
        }

        public override void Render()
        {
            if (HasInputAuthority)
            {
                NetworkObject character = _characterProvider.Character;
                if(character)
                    _camera.SetPosition(character.transform.position);
            }
        }
    }
}