using GlobalVariables;
using PlayerController.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class PlayerAbilityUI : MonoBehaviour
    {
        [Header("Ability Name")]
        [SerializeField] private AbilityTypeReference _playerAbility;
        [SerializeField] private TextMeshProUGUI _abilityNameText;
        
        [Header("Ability Time")]
        [SerializeField] private FloatReference _abilitiesCooldownProgress;
        [SerializeField] private float _timerUpdateSpeed = 1f;
        [SerializeField] private Image _timerImage;
    
        [SerializeField, ReadOnly] private float _target;
    
        private void Start()
        {
            SetAbilityName(_playerAbility);
        }
    
        private void Update()
        {
            _timerImage.fillAmount = Mathf.MoveTowards(
                _timerImage.fillAmount,
                _target,
                _timerUpdateSpeed * Time.deltaTime);
        }
    
        private void OnEnable()
        {
            _playerAbility.AddListener(SetAbilityName);
            _abilitiesCooldownProgress.AddListener(SetTimerProgress);
        }
        
        private void OnDisable()
        {
            _playerAbility.RemoveListener(SetAbilityName);
            _abilitiesCooldownProgress.RemoveListener(SetTimerProgress);
        }
    
        private void SetAbilityName(AbilityType type)
        {
            _abilityNameText.text = type.ToString();
        }
    
        private void SetTimerProgress(float value)
        {
            if (_target <= 0 && value >= 1f)
            {
                _timerImage.fillAmount = 1f;
            }
    
            _target = Mathf.Clamp(value, 0f, 1f);
        }
    }
}