using KrillAudio.Krilloud;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonAudioHandler : MonoBehaviour, ISelectHandler, ISubmitHandler
    {
        [KLVariable, SerializeField] private string _interactionVar;

        private KLAudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<KLAudioSource>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            _audioSource.SetIntVar(_interactionVar, 1);
            _audioSource.Play();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            _audioSource.SetIntVar(_interactionVar, 0);
            _audioSource.Play();
        }
    }
}