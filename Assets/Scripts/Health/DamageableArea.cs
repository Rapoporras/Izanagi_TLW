using UnityEngine;
using Utils.CustomLogs;

namespace Health
{
    public class DamageableArea : MonoBehaviour, IDamageable
    {
        [SerializeField] private EntityHealth _entityHealth;

        [Space(10)]
        [SerializeField] private float _damageMult = 1f;

        private void Start()
        {
            if (!_entityHealth)
            {
                LogManager.LogWarning("Entity health not initialized in Damageable Area", FeatureType.Enemies);
            }
        }

        public void Damage(int amount, bool screenShake)
        {
            if (_entityHealth)
            {
                _entityHealth.Damage(Mathf.CeilToInt(amount * _damageMult), screenShake);
            }
        }

        private void OnValidate()
        {
            FindEntityHealth();
        }

        [ContextMenu("Find Entity Health")]
        private void FindEntityHealth()
        {
            if (_entityHealth) return;

            Transform rootObject = transform.root;
            _entityHealth = rootObject.GetComponent<EntityHealth>();
            if (!_entityHealth)
                _entityHealth = rootObject.GetComponentInChildren<EntityHealth>();
        }
    }
}