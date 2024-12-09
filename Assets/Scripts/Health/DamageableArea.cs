using System;
using UnityEngine;
using Utils.CustomLogs;

namespace Health
{
    public class DamageableArea : MonoBehaviour, IDamageable
    {
        [SerializeField] private EntityHealth _entityHealth;

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
                LogManager.Log("Damage from area", FeatureType.Undefined);
                _entityHealth.Damage(amount, screenShake);
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