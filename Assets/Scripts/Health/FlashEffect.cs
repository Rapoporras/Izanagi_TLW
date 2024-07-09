using System.Collections;
using UnityEngine;

namespace Health
{
    public class FlashEffect : MonoBehaviour
    {
        [Header("Settings")]
        [ColorUsage(true, true)]
        [SerializeField] private Color _flashColor;
        [SerializeField] private float _flashTime;
        [SerializeField] private AnimationCurve _flashSpeedCurve;

        private SpriteRenderer _spriteRenderer;
        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _propertyBlock = new MaterialPropertyBlock();
        }

        [ContextMenu("flash")]
        public void CallDamageFlash()
        {
            StartCoroutine(DamageFlasher());
        }

        private IEnumerator DamageFlasher()
        {
            _spriteRenderer.GetPropertyBlock(_propertyBlock);
            
            // set color
            _propertyBlock.SetColor("_FlashColor", _flashColor);
            _spriteRenderer.SetPropertyBlock(_propertyBlock);
            
            //lerp the flash amount
            float elapsedTime = 0f;
            while (elapsedTime <= _flashTime)
            {
                elapsedTime += Time.deltaTime;
                float currentFlashAmount = Mathf.Lerp(1f, _flashSpeedCurve.Evaluate(elapsedTime), elapsedTime / _flashTime);
                
                _propertyBlock.SetFloat("_FlashAmount", currentFlashAmount);
                _spriteRenderer.SetPropertyBlock(_propertyBlock);
                
                yield return null;
            }
        }
    }
}