using UnityEditor;
using UnityEngine;

namespace CustomAttributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class MinMaxAttribute : PropertyAttribute
    {
        public float min;
        public float max;

        public MinMaxAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
    public class MinMaxDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                MinMaxAttribute range = attribute as MinMaxAttribute;

                EditorGUI.BeginProperty(position, label, property);

                float minValue = property.vector2Value.x;
                float maxValue = property.vector2Value.y;

                float labelWidth = EditorGUIUtility.labelWidth;
                float fieldWidth = 40f;
                float sliderWidth = position.width - labelWidth - fieldWidth * 2 - 20f;
                
                Rect labelRect = new Rect(position.x, position.y, labelWidth, position.height);
                EditorGUI.LabelField(labelRect, label);

                Rect minValueRect = new Rect(labelRect.xMax + 5f, position.y, fieldWidth, position.height);
                minValue = EditorGUI.FloatField(minValueRect, minValue);

                Rect sliderRect = new Rect(minValueRect.xMax + 5f, position.y, sliderWidth, position.height);
                EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, range.min, range.max);
                
                Rect maxValueRect = new Rect(sliderRect.xMax + 5f, position.y, fieldWidth, position.height);
                maxValue = EditorGUI.FloatField(maxValueRect, maxValue);

                property.vector2Value = new Vector2(minValue, maxValue);
                
                EditorGUI.EndProperty();
            }
        }
    }
#endif
}