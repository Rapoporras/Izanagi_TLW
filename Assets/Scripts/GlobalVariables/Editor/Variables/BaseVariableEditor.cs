using UnityEditor;
using UnityEngine;

namespace GlobalVariables.Editor
{
    public abstract class BaseVariableEditor<T, V> : UnityEditor.Editor where V : BaseVariable<T>
    {
        protected SerializedProperty _variableValue;
        
        private void OnEnable()
        {
            _variableValue = serializedObject.FindProperty("_value");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            V variable = target as V;
            GUILayout.Space(10);
            if (GUILayout.Button("Update"))
            {
                T value = GetValue();
                variable.Value = value;
            }
        }

        protected abstract T GetValue();
    }
}