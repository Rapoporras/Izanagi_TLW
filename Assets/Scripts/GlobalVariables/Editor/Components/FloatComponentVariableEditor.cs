using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(FloatComponentVariable))]
    public class FloatComponentVariableEditor : BaseComponentVariableEditor<float, FloatComponentVariable>
    {
        protected override float GetValue()
        {
            return _variableValue.floatValue;
        }
    }
}