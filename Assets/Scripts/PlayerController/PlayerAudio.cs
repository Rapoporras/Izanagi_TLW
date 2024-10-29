using KrillAudio.Krilloud;
using UnityEngine;

namespace PlayerController
{
    public class PlayerAudio : MonoBehaviour
    {
        [Header("Settings")]
        [KLTag, SerializeField] private string _attackTag;
        [KLVariable, SerializeField] private string _attackVariable;
        [Space(10)]
        [KLTag, SerializeField] private string _stateTag;
        [KLVariable, SerializeField] private string _stateVariable;
        [Space(10)]
        [KLTag, SerializeField] private string _fireAbilityTag;
        [KLVariable, SerializeField] private string _abilityActionVariable;
        
        private KLAudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<KLAudioSource>();
        }

        public void PlayAttackSound(int combo)
        {
            _audioSource.SetIntVar(_attackVariable, combo);
            _audioSource.Play(_attackTag);
        }

        public void PlayJumpSound() => PlayStateSound(0);
        public void PlayFallSound() => PlayStateSound(1);
        public void PlayDashSound() => PlayStateSound(2);
        public void PlayWalkSound() => PlayStateSound(3);
        public void PlayRecoverHealthSound() => PlayStateSound(4);
        public void PlayChangeAbilitySound() => PlayStateSound(5);
        public void PlayHitSound() => PlayStateSound(6);

        public void StopWalkSound()
        {
            _audioSource.Stop();
        }
        
        private void PlayStateSound(int state)
        {
            _audioSource.SetIntVar(_stateVariable, state);
            _audioSource.Play(_stateTag);
        }
    }
}