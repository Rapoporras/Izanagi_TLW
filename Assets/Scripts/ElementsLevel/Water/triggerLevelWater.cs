using System.Collections;
using System.Collections.Generic;
using InteractionSystem;
using UnityEngine;
using UnityEngine.UI;
namespace DemoScripts
{
    public class triggerLevelWater : MonoBehaviour, IInteractable

    {
        [SerializeField] private GameObject tilemapWaterDown;
        [SerializeField] private GameObject tilemapWaterUP;

        public Image fadeImage; // Asigna aquí la imagen del Canvas para el fade
        public float fadeDuration = 1.0f; // Duración del fade

        [SerializeField] private GameObject _interactionText;

        private void Start()
        {
            // Asegúrate de que la imagen esté completamente transparente al inicio
            SetImageAlpha(0);
        }

        public void Interact(Interactor interactor)
        {
            StartFadeAndChangeAssets();
        }

        public void ShowInteractionUI(bool showUI)
        {
            _interactionText.SetActive(showUI);
        }



        // Método para iniciar el fade y cambiar los assets
        public void StartFadeAndChangeAssets()
        {
            StartCoroutine(FadeOutAndIn());
        }

        private IEnumerator FadeOutAndIn()
        {
            // Fade out
            yield return Fade(1);

            // Cambiar los assets o desactivar/activar los objetos deseados
            ChangeAssets();

            // Fade in
            yield return Fade(0);
        }

        private IEnumerator Fade(float targetAlpha)
        {
            float startAlpha = fadeImage.color.a;
            float elapsed = 0;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
                SetImageAlpha(newAlpha);
                yield return null;
            }

            SetImageAlpha(targetAlpha); // Asegura que el alpha llegue al valor final
        }

        private void SetImageAlpha(float alpha)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }

        private void ChangeAssets()
        {
            tilemapWaterDown.SetActive(false);
            tilemapWaterUP.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}