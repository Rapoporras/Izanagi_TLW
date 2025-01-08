using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonAudioHandler : MonoBehaviour, ISelectHandler, ISubmitHandler
    {

        private AudioSource _audioSource;
        [SerializeField] private AudioClip _onHoverAudio;
        [SerializeField] private AudioClip _onClickAudio;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            _audioSource.clip = _onHoverAudio;
            _audioSource.Play();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            _audioSource.clip = _onClickAudio;
            _audioSource.Play();
        }
    }
}