using GlobalVariables;
using UnityEngine;

namespace Items
{
    public class ItemManager : MonoBehaviour
    {
        [Header("Upgrade Settings")]
        [SerializeField] private int _healthItemsAmountToUpgrade = 4;
        [SerializeField] private int _attackItemsAmountToUpgrade = 4;
        [Space(5)]
        [SerializeField] private int _healthAmountToAdd = 10;
        [SerializeField] private float _attackMultiplierToAdd = 0.1f;
        
        [Header("Dependencies")]
        [SerializeField] private IntReference _healthItems;
        [SerializeField] private IntReference _attackItems;
        [Space(5)]
        [SerializeField] private IntReference _playerMaxHealth;
        [SerializeField] private IntReference _playerCurrentHealth;
        [Space(5)]
        [SerializeField] private FloatReference _attackMultiplier;

        private static ItemManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void UpdateHealthData(int amount) // called from listener
        {
            if (amount >= _healthItemsAmountToUpgrade)
            {
                _playerMaxHealth.Value += _healthAmountToAdd;
                _playerCurrentHealth.Value = _playerMaxHealth;

                _healthItems.Value -= _healthItemsAmountToUpgrade;
            }
        }
        
        public void UpdateAttackData(int amount) // called from listener
        {
            if (amount >= _attackItemsAmountToUpgrade)
            {
                _attackMultiplier.Value += _attackMultiplierToAdd;

                _attackItems.Value -= _attackItemsAmountToUpgrade;
            }
        }
    }
}