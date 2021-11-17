using UnityEngine;

namespace RPG.Core {
    [CreateAssetMenu(menuName = "Weapon", fileName = "New Weapon")]
    public class WeaponSO : ScriptableObject {
        [SerializeField] GameObject _projectile;
        [SerializeField] float _reloadTime;
        public GameObject Projectile { get => _projectile; }
        public float ReloadTime { get => _reloadTime; }
    }
}