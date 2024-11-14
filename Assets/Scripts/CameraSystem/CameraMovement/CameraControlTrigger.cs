using Cinemachine;
using UnityEditor;
using UnityEngine;

namespace CameraSystem
{
    public class CameraControlTrigger : MonoBehaviour
    {
        public bool swapCameras;
        public bool panCameraOnContact;
        public bool zoomCameraOnContact;

        // swap parameters
        public CinemachineVirtualCamera cameraOnLeft;
        public CinemachineVirtualCamera cameraOnRight;

        // pan parameters
        public PanDirection panDirection;
        public float panDistance = 3f;
        public float panDuration = 0.35f;
        
        // zoom parameters
        public float zoomDuration;
        public float zoomValue;

        private Collider2D _coll;

        private void Start()
        {
            _coll = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (panCameraOnContact)
                {
                    CameraManager.Instance.PanCameraOnContact(panDistance, panDuration, panDirection, false);
                }

                if (zoomCameraOnContact)
                {
                    CameraManager.Instance.ZoomCameraOnContact(zoomValue, zoomDuration, false);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Vector2 exitDirection = (other.transform.position - _coll.bounds.center).normalized;
                if (swapCameras && cameraOnLeft != null && cameraOnRight != null)
                {
                    CameraManager.Instance.SwapCamera(cameraOnLeft, cameraOnRight, exitDirection);
                }
                
                if (panCameraOnContact)
                {
                    CameraManager.Instance.PanCameraOnContact(panDistance, panDuration, panDirection, true);
                }
                
                if (zoomCameraOnContact)
                {
                    CameraManager.Instance.ZoomCameraOnContact(zoomValue, zoomDuration, true);
                }
            }
        }
    }

    public enum PanDirection
    {
        Up, Down, Left, Right
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(CameraControlTrigger))]
    public class CameraControlTriggerEditor : Editor
    {
        private CameraControlTrigger _cameraControl;

        private void OnEnable()
        {
            _cameraControl = target as CameraControlTrigger;
        }

        public override void OnInspectorGUI()
        {
            SetSwapCameraFields();
            EditorGUILayout.Space();
            SetPanCameraFields();
            EditorGUILayout.Space();
            SetZoomCameraFields();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_cameraControl);
            }
        }

        private void SetSwapCameraFields()
        {
            _cameraControl.swapCameras = EditorGUILayout.Toggle("Swap Camera",
                _cameraControl.swapCameras);

            if (_cameraControl.swapCameras)
            {
                EditorGUI.indentLevel++;

                _cameraControl.cameraOnLeft = EditorGUILayout.ObjectField(
                    "Camera on Left",
                    _cameraControl.cameraOnLeft,
                    typeof(CinemachineVirtualCamera),
                    true) as CinemachineVirtualCamera;
                _cameraControl.cameraOnRight = EditorGUILayout.ObjectField(
                    "Camera on Right",
                    _cameraControl.cameraOnRight,
                    typeof(CinemachineVirtualCamera),
                    true) as CinemachineVirtualCamera;
                
                EditorGUI.indentLevel--;
            }
        }

        private void SetPanCameraFields()
        {
            _cameraControl.panCameraOnContact = EditorGUILayout.Toggle("Pan Camera On Contact",
                _cameraControl.panCameraOnContact);
            
            if (_cameraControl.panCameraOnContact)
            {
                EditorGUI.indentLevel++;
                
                _cameraControl.panDirection =
                    (PanDirection) EditorGUILayout.EnumPopup("Camera Pan Direction", _cameraControl.panDirection);
                _cameraControl.panDistance = EditorGUILayout.FloatField("Camera Pan Distance", _cameraControl.panDistance);
                _cameraControl.panDuration = EditorGUILayout.FloatField("Camera Pan Duration", _cameraControl.panDuration);
                
                EditorGUI.indentLevel--;
            }
        }

        private void SetZoomCameraFields()
        {
            _cameraControl.zoomCameraOnContact = EditorGUILayout.Toggle("Zoom Camera On Contact",
                _cameraControl.zoomCameraOnContact);

            if (_cameraControl.zoomCameraOnContact)
            {
                EditorGUI.indentLevel++;

                _cameraControl.zoomValue = Mathf.Max(0.1f,
                    EditorGUILayout.FloatField("Zoom", _cameraControl.zoomValue));
                _cameraControl.zoomDuration = Mathf.Max(0,
                    EditorGUILayout.FloatField("Zoom Duration", _cameraControl.zoomDuration));

                EditorGUI.indentLevel--;
            }
        }
    }
#endif
}