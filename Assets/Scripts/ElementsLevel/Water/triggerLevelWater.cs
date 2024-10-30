using System.Collections;
using InteractionSystem;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace DemoScripts
{
    public class triggerLevelWater : IdentifiableObject, IInteractable, IDataPersistence
    {
        [SerializeField] private GameObject tilemapWaterDown;
        [SerializeField] private GameObject tilemapWaterUP;

        public Image fadeImage; // Asigna aquí la imagen del Canvas para el fade
        public float fadeDuration = 1.0f; // Duración del fade

        [SerializeField] private GameObject _interactionText;

        private bool _eventActivated;

        private void Start()
        {
            // Asegúrate de que la imagen esté completamente transparente al inicio
            SetImageAlpha(0);
        }

        public void Interact(Interactor interactor)
        {
            if (_eventActivated) return;

            _eventActivated = true;
            StartFadeAndChangeAssets();
        }

        public void ShowInteractionUI(bool showUI)
        {
            if (_eventActivated) return;
            
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

            // Desactiva el GameObject que contiene este script
            gameObject.SetActive(false);
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
        }

        public void LoadData(GameData data)
        {
            data.sceneEvents.TryGetValue(id, out _eventActivated);
            if (_eventActivated)
            {
                ChangeAssets();
            }
        }

        public void SaveData(ref GameData data)
        {
            if (data.sceneEvents.ContainsKey(id))
            {
                data.sceneEvents.Remove(id);
            }
            data.sceneEvents.Add(id, _eventActivated);
        }
    }
}