using GlobalVariables;
using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private IntReference _maxHealth;
    [SerializeField] private IntReference _currentHealth;

    [SerializeField] private TextMeshProUGUI _healthText;

    private void Start()
    {
        UpdateHealthText(_currentHealth.Value);
    }

    private void OnEnable()
    {
        _currentHealth.variableValue.OnValueChanged += UpdateHealthText;
    }

    private void OnDisable()
    {
        _currentHealth.variableValue.OnValueChanged -= UpdateHealthText;
    }

    private void UpdateHealthText(int value)
    {
        _healthText.text = $"{value} / {_maxHealth.Value}";
    }
}
