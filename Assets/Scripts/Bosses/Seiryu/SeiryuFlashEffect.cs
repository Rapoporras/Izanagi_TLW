using System;
using Health;
using UnityEngine;

namespace Bosses
{
    public class SeiryuFlashEffect: MonoBehaviour
    {
        [SerializeField] private EntityHealth _entityHealth;
        
        private FlashEffect _flashEffect;

        private void Awake()
        {
            _flashEffect = GetComponent<FlashEffect>();
        }

        private void OnEnable()
        {
            _entityHealth.AddListenerOnHit(DamageFlasher);
        }
        
        private void OnDisable()
        {
            _entityHealth.RemoveListenerOnHit(DamageFlasher);
        }

        private void DamageFlasher()
        {
            _flashEffect.CallDamageFlash();
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