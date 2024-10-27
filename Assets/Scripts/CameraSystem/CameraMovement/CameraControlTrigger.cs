using System;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraSystem
{
    public class CameraControlTrigger : MonoBehaviour
    {
        public bool swapCameras = false;
        public bool panCameraOnContact = false;

        [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
        [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

        [HideInInspector] public PanDirection panDirection;
        [HideInInspector] public float panDistance = 3f;
        [HideInInspector] public float panDuration = 0.35f;

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

            EditorGUILayout.Space();
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

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_cameraControl);
            }
        }
    }
#endif
}