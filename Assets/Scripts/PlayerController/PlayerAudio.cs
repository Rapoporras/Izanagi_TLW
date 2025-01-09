using UnityEngine;

namespace PlayerController
{
    public class PlayerAudio : MonoBehaviour
    {
        
        [SerializeField] private AudioClip firstAttackSound;
        [SerializeField] private AudioClip secondAttackSound;
        [SerializeField] private AudioClip thirdAttackSound;
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip fallSound;
        [SerializeField] private AudioClip dashSound;
        [SerializeField] private AudioClip walkSound;
        [SerializeField] private AudioClip recoverHealthSound;
        [SerializeField] private AudioClip hitSound;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayAttackSound(int combo)
        {
            if (combo == 0)
            {
                _audioSource.clip = firstAttackSound;
            } else if (combo == 1)
            {
                _audioSource.clip = secondAttackSound;
            }
            else
            {
                _audioSource.clip = thirdAttackSound;
            }
            
            _audioSource.Play();
        }

        public void PlayJumpSound()
        {
            _audioSource.clip = jumpSound;
            _audioSource.Play();
        }

        public void PlayFallSound()
        {
            _audioSource.clip = fallSound;
            _audioSource.Play();
        }

        public void PlayDashSound()
        {
            _audioSource.clip = dashSound;
            _audioSource.Play();
        }

        public void PlayWalkSound()
        {
            _audioSource.clip = walkSound;
            _audioSource.Play();
        }

        public void PlayRecoverHealthSound()
        {
            _audioSource.clip = recoverHealthSound;
            _audioSource.Play();
        }
        
        public void PlayHitSound()
        {
            _audioSource.clip = hitSound;
            _audioSource.Play();
        }

        public void StopWalkSound()
        {
            _audioSource.Stop();
        }
        
    }
}