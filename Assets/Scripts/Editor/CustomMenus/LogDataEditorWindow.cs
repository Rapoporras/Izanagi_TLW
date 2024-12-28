using UnityEditor;
using Utils.CustomLogs;
using UnityEngine;

namespace CustomMenus
{
    public class LogDataEditorWindow : EditorWindow
    {
        private FeatureLogScriptable _logData;

        [MenuItem("IzanagiTLW/LogData")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LogDataEditorWindow), true, "Log Data");
        }

        private void OnEnable()
        {
            if (_logData == null)
            {
                _logData = (FeatureLogScriptable) AssetDatabase.LoadAssetAtPath(LogManager.LOGS_PATH,
                    typeof(FeatureLogScriptable));
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Log Data Configuration", EditorStyles.boldLabel);
            
            _logData = (FeatureLogScriptable)EditorGUILayout.ObjectField("Log Data", _logData, typeof(FeatureLogScriptable), false);
            if (_logData && _logData.GetType().GetField("_features", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) != null)
            {
                SerializedObject serializedObject = new SerializedObject(_logData);
                SerializedProperty featuresProperty = serializedObject.FindProperty("_features");

                EditorGUILayout.PropertyField(featuresProperty, true);

                if (GUILayout.Button("Guardar Cambios"))
                {
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(_logData);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}