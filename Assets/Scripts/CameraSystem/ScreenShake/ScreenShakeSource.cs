using Cinemachine;
using GameEvents;
using UnityEngine;

namespace CameraSystem
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ScreenShakeSource : MonoBehaviour
    {
        [SerializeField] private ScreenShakeProfile _screenShakeProfile;
        [SerializeField] private ScreenShakeDataEvent _screenShakeEvent;
        
        private CinemachineImpulseSource _impulseSource;
        private ScreenShakeData _screenShakeData;

        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void Start()
        {
            _screenShakeData.profile = _screenShakeProfile;
            _screenShakeData.impulseSource = _impulseSource;
        }

        public void TriggerScreenShake()
        {
            if (_screenShakeEvent)
                _screenShakeEvent.Raise(_screenShakeData);
        }
    }
}