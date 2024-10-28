using GlobalVariables;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private IntReference _maxHealth;
        [SerializeField] private IntReference _currentHealth;
        [SerializeField] private float _updateSpeed;

        [SerializeField] private Image _healthBarImage;

        private float _targetValue;

        private void Start()
        {
            UpdateHealthText(_currentHealth.Value);
            _healthBarImage.fillAmount = _targetValue;
        }

        private void Update()
        {
            _healthBarImage.fillAmount = Mathf.MoveTowards(
                _healthBarImage.fillAmount,
                _targetValue,
                _updateSpeed * Time.deltaTime);
        }

        private void OnEnable()
        {
            _currentHealth.AddListener(UpdateHealthText);
        }

        private void OnDisable()
        {
            _currentHealth.RemoveListener(UpdateHealthText);
        }

        private void UpdateHealthText(int value)
        {
            _targetValue = (float)value / _maxHealth.Value;
        }
    }
}
