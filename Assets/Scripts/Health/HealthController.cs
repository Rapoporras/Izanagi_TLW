using System;
using UnityEngine;

namespace Health
{
    [Serializable]
    public class HealthController
    {
        [field: SerializeField, ReadOnly] public int MaxHealth { get; private set; }
        [field: SerializeField, ReadOnly] public int CurrentHealth { get; private set; }

        public event Action OnDeathEvent = delegate { };
        public event Action OnHealthUpdated = delegate { };

        public HealthController()
        {
            // default values
            MaxHealth = 1;
            CurrentHealth = 1;
        }
        
        public HealthController(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        private void UpdateHealth(int amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            OnHealthUpdated.Invoke();
            if (CurrentHealth == 0)
            {
                OnDeathEvent.Invoke();
            }
        }

        public void Damage(int amount)
        {
            UpdateHealth(-amount);
        }

        public void Recover(int amount)
        {
            UpdateHealth(amount);
        }

        /// <summary>
        /// Reset MaxHealth and CurrentHealth with the new value
        /// </summary>
        /// <param name="newMaxHealth">New health value</param>
        public void ResetHealth(int newMaxHealth)
        {
            MaxHealth = newMaxHealth;
            CurrentHealth = newMaxHealth;
        }
    }
}
