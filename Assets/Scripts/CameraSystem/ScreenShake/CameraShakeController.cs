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

        public void ScreenShakeFromProfile(ScreenShakeData data)
        {
            // apply settings
            SetupScreenShakeSettings(data);
            
            // screenshake
            data.impulseSource.GenerateImpulse(data.profile.impulseForce);
        }

        private void SetupScreenShakeSettings(ScreenShakeData data)
        {
            _impulseDefinition = data.impulseSource.m_ImpulseDefinition;

            // impulse source settings
            _impulseDefinition.m_ImpulseDuration = data.profile.impulseTime;
            data.impulseSource.m_DefaultVelocity = data.profile.defaultVelocity;

            if (data.profile.impulseShape == CinemachineImpulseDefinition.ImpulseShapes.Custom)
            {
                _impulseDefinition.m_CustomImpulseShape = data.profile.impulseCurve;
            }
            else
            {
                _impulseDefinition.m_ImpulseShape = data.profile.impulseShape;
            }
            
            // impulse listener settings
            _impulseListener.m_ReactionSettings.m_AmplitudeGain = data.profile.listenerAmplitude;
            _impulseListener.m_ReactionSettings.m_FrequencyGain = data.profile.listenerFrequency;
            _impulseListener.m_ReactionSettings.m_Duration = data.profile.listenerDuration;
        }
    }
}