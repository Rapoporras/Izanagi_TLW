using System.Collections;
using PlayerController.Abilities;
using TMPro;
using UnityEngine;

namespace Abilities
{
    public class AbilityUnlockedUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _abilityText;
        [SerializeField] private GameObject _textPanel;
        [SerializeField] private float _textDuration;

        private bool _isShowingText;

        public void ShowAbilityUnlockedText(AbilityType ability)
        {
            if (_isShowingText) return;
            StartCoroutine(ShowTextCoroutine(ability, _textDuration));
        }

        private IEnumerator ShowTextCoroutine(AbilityType ability, float duration)
        {
            _isShowingText = true;
            InputManager.Instance.DisablePlayerActions();
            _abilityText.text = $"{ability.ToString().ToUpper()} ABILITY UNLOCKED";
            _textPanel.SetActive(true);
            _abilityText.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(duration);
            
            InputManager.Instance.EnablePlayerActions();
            _textPanel.SetActive(false);
            _abilityText.gameObject.SetActive(false);
            _isShowingText = false;
        }
    }
}
