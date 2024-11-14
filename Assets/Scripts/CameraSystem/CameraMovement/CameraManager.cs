using System.Collections;
using Cinemachine;
using UnityEngine;

namespace CameraSystem
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Cameras")]
        [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;
        
        [Header("Settings")]
        [SerializeField] private float _fallPanAmount = 0.25f;
        [SerializeField] private float _fallYPanTime = 0.35f;
        public float fallSpeedYDampingChangeThreshold = -15f;
        
        public bool IsLerpingYDamping { get; private set; }
        public bool LerpedFromPlayerFalling { get; set; }

        private Coroutine _lerpYPanCoroutine;
        private Coroutine _panCameraCoroutine;
        private Coroutine _zoomCameraCoroutine;

        private CinemachineVirtualCamera _currentCamera;
        private CinemachineFramingTransposer _framingTransposer;

        private float _normYPanAmount;

        private Vector2 _startingTrackedObjectOffset;
        private float _startingOrthographicSize;
        
        public static CameraManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }


            for (int i = 0; i < _allVirtualCameras.Length; i++)
            {
                if (_allVirtualCameras[i].enabled)
                {
                    // set the current active camera
                    _currentCamera = _allVirtualCameras[i];
                    _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                }
            }

            _normYPanAmount = _framingTransposer.m_YDamping;
            _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
            _startingOrthographicSize = _currentCamera.m_Lens.OrthographicSize;
        }
        
        #region LERP THE Y DAMPING
        public void LerpYDamping(bool isPlayerFalling)
        {
            if (_lerpYPanCoroutine != null)
                StopCoroutine(_lerpYPanCoroutine);
            _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
        }

        private IEnumerator LerpYAction(bool isPlayerFalling)
        {
            IsLerpingYDamping = true;

            float startDampAmount = _framingTransposer.m_YDamping;
            float endDampAmount;

            if (isPlayerFalling)
            {
                endDampAmount = _fallPanAmount;
                LerpedFromPlayerFalling = true;
            }
            else
            {
                endDampAmount = _normYPanAmount;
            }

            float elapsedTime = 0f;
            while (elapsedTime < _fallYPanTime)
            {
                elapsedTime += Time.deltaTime;

                float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / _fallYPanTime);
                _framingTransposer.m_YDamping = lerpedPanAmount;
                
                yield return null;
            }

            IsLerpingYDamping = false;
        }
        #endregion
        
        #region PAN CAMERA
        public void PanCameraOnContact(float distance, float duration, PanDirection direction, bool panToStartingPos)
        {
            if (_panCameraCoroutine != null)
                StopCoroutine(_panCameraCoroutine);
            _panCameraCoroutine = StartCoroutine(PanCamera(distance, duration, direction, panToStartingPos));
        }

        private IEnumerator PanCamera(float distance, float duration, PanDirection direction, bool panToStartingPos)
        {
            Vector2 endPos = Vector2.zero;
            Vector2 startingPos;

            if (!panToStartingPos)
            {
                switch (direction)
                {
                    case PanDirection.Up:
                        endPos = Vector2.up;
                        break;
                    case PanDirection.Down:
                        endPos = Vector2.down;
                        break;
                    case PanDirection.Left:
                        endPos = Vector2.left;
                        break;
                    case PanDirection.Right:
                        endPos = Vector2.right;
                        break;
                }

                endPos *= distance;
                startingPos = _startingTrackedObjectOffset;
                endPos += startingPos;
            }
            else
            {
                startingPos = _framingTransposer.m_TrackedObjectOffset;
                endPos = _startingTrackedObjectOffset;
            }

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                Vector3 panLerp = Vector3.Lerp(startingPos, endPos, elapsedTime / duration);
                _framingTransposer.m_TrackedObjectOffset = panLerp;

                yield return null;
            }
        }
        #endregion
        
        #region SWAP CAMERAS
        public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
        {
            if (_currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
            {
                cameraFromRight.enabled = true;
                cameraFromLeft.enabled = false;
                
                _currentCamera = cameraFromRight;
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
            else if (_currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
            {
                cameraFromRight.enabled = false;
                cameraFromLeft.enabled = true;
                
                _currentCamera = cameraFromLeft;
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        #endregion
        
        #region CAMERA ZOOM
        public void ZoomCameraOnContact(float zoom, float duration, bool zoomToStartingValue)
        {
            if (_zoomCameraCoroutine != null)
                StopCoroutine(_zoomCameraCoroutine);
            _zoomCameraCoroutine = StartCoroutine(ZoomCamera(zoom, duration, zoomToStartingValue));
        }

        private IEnumerator ZoomCamera(float zoom, float duration, bool zoomToStartingValue)
        {
            float startValue;
            float endValue;

            if (!zoomToStartingValue)
            {
                startValue = _startingOrthographicSize;
                endValue = startValue / zoom;
            }
            else
            {
                endValue = _startingOrthographicSize;
                startValue = endValue / zoom;
            }
            
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                
                float orthoSizeLerp = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                _currentCamera.m_Lens.OrthographicSize = orthoSizeLerp;
                
                yield return null;
            }
        }
        #endregion
    }
}