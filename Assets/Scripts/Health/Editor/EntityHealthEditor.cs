using UnityEditor;
using UnityEngine;


namespace Health
{
    [CustomEditor(typeof(EntityHealth))]
    public class EntityHealthEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EntityHealth entityHealth = target as EntityHealth;
            
            entityHealth.giveUpwardForce = EditorGUILayout.Toggle("Give Upward Force", entityHealth.giveUpwardForce);
            entityHealth.damageable = EditorGUILayout.Toggle("Damageable", entityHealth.damageable);

            if (entityHealth.damageable)
            {
                EditorGUI.indentLevel++;
                
                entityHealth.maxHealth = EditorGUILayout.IntField("Max Health", entityHealth.maxHealth);
                entityHealth.invulnerabilityTime = EditorGUILayout.FloatField("Invulnerability Time", entityHealth.invulnerabilityTime);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_health"), true);
                
                EditorGUI.indentLevel--;
            }
        }
    }
}
