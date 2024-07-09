using GlobalVariables;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerMannaUI : MonoBehaviour
    {
        [Header("Player Manna")]
        [SerializeField] private IntReference _currentManna;
        [SerializeField] private IntReference _maxManna;
        [SerializeField] private float _updateSpeed;

        [Header("UI Elements")]
        [SerializeField] private Image _imageContainer;

        private float _targetValue;

        private void Start()
        {
            _targetValue = (float)_currentManna / _maxManna;
            _imageContainer.fillAmount = _targetValue;
        }

        private void Update()
        {
            _imageContainer.fillAmount = Mathf.MoveTowards(
                _imageContainer.fillAmount,
                _targetValue,
                _updateSpeed * Time.deltaTime);
        }

        private void OnEnable()
        {
            _currentManna.AddListener(UpdateManna);
        }

        private void OnDisable()
        {
            _currentManna.RemoveListener(UpdateManna);
        }

        private void UpdateManna(int value)
        {
            _targetValue = (float)value / _maxManna;
        }
    }
}