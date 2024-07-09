using Cinemachine;
using GameEvents;
using UnityEngine;

namespace CameraSystem
{
    [RequireComponent(typeof(ScreenShakeDataListener))]
    public class CameraShakeController : MonoBehaviour
    {
        private CinemachineImpulseListener _impulseListener;
        private CinemachineImpulseDefinition _impulseDefinition;

        private void Awake()
        {
            _impulseListener = GetComponent<CinemachineImpulseListener>();
        }

        public void ScreenShakeFromProfile(ScreenShakeData context)
        {
            // apply settings
            SetupScreenShakeSettings(context);
            
            // screenshake
            context.impulseSource.GenerateImpulse(context.profile.impulseForce);
        }

        private void SetupScreenShakeSettings(ScreenShakeData context)
        {
            _impulseDefinition = context.impulseSource.m_ImpulseDefinition;

            // impulse source settings
            _impulseDefinition.m_ImpulseDuration = context.profile.impulseTime;
            context.impulseSource.m_DefaultVelocity = context.profile.defaultVelocity;
            _impulseDefinition.m_CustomImpulseShape = context.profile.impulseCurve;
            
            // impulse listener settings
            _impulseListener.m_ReactionSettings.m_AmplitudeGain = context.profile.listenerAmplitude;
            _impulseListener.m_ReactionSettings.m_FrequencyGain = context.profile.listenerFrequency;
            _impulseListener.m_ReactionSettings.m_Duration = context.profile.listenerDuration;
        }
    }
}