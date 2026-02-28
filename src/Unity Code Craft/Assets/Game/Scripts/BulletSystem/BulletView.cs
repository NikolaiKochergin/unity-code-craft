using UnityEngine;

namespace Game
{
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private BulletViewConfig _config;
        
        [SerializeField] private GameObject _blueVFX;
        [SerializeField] private GameObject _redVFX;

        private void Awake() => _bullet.OnDamageApplied += OnDamageApplied;

        private void OnEnable() => SetVisual(_bullet.Team);

        private void OnDestroy() => _bullet.OnDamageApplied -= OnDamageApplied;

        private void SetVisual(TeamType team)
        {
            _blueVFX.SetActive(team == TeamType.Player);
            _redVFX.SetActive(team != TeamType.Player);
        }

        private void OnDamageApplied(Bullet bullet) => 
            Instantiate(_config.ExplosionVFX, bullet.Position, Quaternion.identity);
    }
}