using UnityEditor;
using UnityEngine;

namespace GlobalVariables.Editor
{
    public class BaseReferenceDrawer : PropertyDrawer
    {
        /// <summary>
        /// Options to display in the popup to select constant or variable.
        /// </summary>
        private readonly string[] popupOptions = 
            { "Use Constant", "Use Variable", "Use Component" };

        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            
            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty valueType = property.FindPropertyRelative("valueType");
            SerializedProperty constantValue = property.FindPropertyRelative("constantValue");
            SerializedProperty variable = property.FindPropertyRelative("variableValue");
            SerializedProperty component = property.FindPropertyRelative("componentValue");

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            int result = EditorGUI.Popup(buttonRect, valueType.enumValueIndex, popupOptions, popupStyle);
            
            valueType.enumValueIndex = result;

            SerializedProperty selectedProperty = constantValue;
            if (valueType.enumValueIndex == 1)
                selectedProperty = variable;
            else if (valueType.enumValueIndex == 2)
                selectedProperty = component;

            EditorGUI.PropertyField(position, selectedProperty, GUIContent.none);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}