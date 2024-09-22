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
                // DontDestroyOnLoad(this);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            _healthItems.AddListener(UpdateHealthData);
            _attackItems.AddListener(UpdateAttackData);
        }

        private void OnDisable()
        {
            _healthItems.RemoveListener(UpdateHealthData);
            _attackItems.RemoveListener(UpdateAttackData);
        }

        private void UpdateHealthData(int amount)
        {
            if (amount % _healthItemsAmountToUpgrade == 0)
            {
                // upgrade health
                _playerMaxHealth.Value += _healthAmountToAdd;
                _playerCurrentHealth.Value = _playerMaxHealth;
            }
        }
        
        private void UpdateAttackData(int amount)
        {
            if (amount % _attackItemsAmountToUpgrade == 0)
            {
                // upgrade attack
                _attackMultiplier.Value += _attackMultiplierToAdd;
            }
        }
    }
}