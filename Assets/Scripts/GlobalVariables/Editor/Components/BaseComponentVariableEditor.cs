using UnityEditor;
using UnityEngine;

namespace GlobalVariables.Editor
{
    public abstract class BaseComponentVariableEditor<T, C> : UnityEditor.Editor where C : BaseComponentVariable<T>
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

            C component = target as C;
            GUILayout.Space(10);
            if (GUILayout.Button("Update"))
            {
                T value = GetValue();
                component.Value = value;
            }
        }

        protected abstract T GetValue();
    }
}