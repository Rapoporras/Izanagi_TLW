using System;
using UnityEngine;

namespace Health
{
    [Serializable]
    public class HealthController
    {
        [field: SerializeField, ReadOnly] public int MaxHealth { get; private set; }
        [field: SerializeField, ReadOnly] public int CurrentHealth { get; private set; }

        public event Action OnDeathEvent;
        public event Action OnHealthUpdated;

        public HealthController(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        private void UpdateHealth(int amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            OnHealthUpdated?.Invoke();
            if (CurrentHealth == 0)
            {
                OnDeathEvent?.Invoke();
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
    }
}
