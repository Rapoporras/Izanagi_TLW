using KrillAudio.Krilloud;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerController
{
    public class KappaAudio : MonoBehaviour
    {
        [FormerlySerializedAs("_kappaStateTag")]
        [Header("Settings")]
        [KLTag, SerializeField] private string _actionTag;
        [KLVariable, SerializeField] private string _actionVariable;
        
        private KLAudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<KLAudioSource>();
        }

        public void PlayRollSound() => PlayStateSound(0);
        public void PlayWalkSound() => PlayStateSound(1);
        public void PlayAttackSound() => PlayStateSound(2);
        public void PlayRollHitSound() => PlayStateSound(3);
        public void PlayRangeAttackSound() => PlayStateSound(4);
        public void PlayHitSound() => PlayStateSound(6);
        
        public void StopSounds()
        {
            _audioSource.Stop();
        }
        
        private void PlayStateSound(int state)
        {
            _audioSource.SetIntVar(_actionVariable, state);
            _audioSource.Play(_actionTag);
        }
    }
}