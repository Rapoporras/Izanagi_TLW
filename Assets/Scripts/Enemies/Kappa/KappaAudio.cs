using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerController
{
    public class KappaAudio : MonoBehaviour
    {
        
        [SerializeField] private AudioClip rollSound;
        [SerializeField] private AudioClip walkSound;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioClip rollHitSound;
        [SerializeField] private AudioClip hitSound;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        
        private void PlayRollSound()
        {
            _audioSource.clip = rollSound;
            _audioSource.Play();
        }
        
        public void PlayWalkSound()
        {
            _audioSource.clip = walkSound;
            _audioSource.Play();
        }

        public void PlayAttackSound()
        {
            _audioSource.clip = attackSound;
            _audioSource.Play();
        }

        public void PlayRollHitSound()
        {
            _audioSource.clip = rollHitSound;
            _audioSource.Play();
        }

        public void PlayHitSound()
        {
            _audioSource.clip = hitSound;
            _audioSource.Play();
        }
        
        public void StopSounds()
        {
            _audioSource.Stop();
        }
    }
}