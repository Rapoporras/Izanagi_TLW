using GlobalVariables;
using PlayerController.Data;
using SaveSystem;
using UnityEngine;

namespace Managers
{
    public class VariablesCollector : MonoBehaviour, IDataPersistence
    {
        [Header("Health Variables")]
        [SerializeField] private IntReference _playerMaxHealth;
        [SerializeField] private IntReference _playerMaxPotions;
        
        [Header("Items Variables")]
        [SerializeField] private IntReference _attackItemAmount;
        [SerializeField] private IntReference _healthItemAmount;
        
        [Header("Player Attack")]
        [SerializeField] private FloatReference _attackMultiplier;
        
        [Header("Player Data")]
        [SerializeField] private PlayerAbilitiesData _playerAbilitiesData;

        private static VariablesCollector _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }


        // don't need to load any data
        public void LoadData(GameData data) { }

        public void SaveData(ref GameData data)
        {
            data.variables.maxHealth = _playerMaxHealth;
            data.variables.maxPotions = _playerMaxPotions;
            
            data.variables.attackItemAmount = _attackItemAmount;
            data.variables.healthItemAmount = _healthItemAmount;

            data.variables.attackMultiplier = Mathf.Round(_attackMultiplier * 100f) / 100f;

            data.abilitiesUnlocked = _playerAbilitiesData.GetAbilitiesList();
        }
    }
}