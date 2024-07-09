using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(FloatVariable))]
    public class FloatVariableEditor : BaseVariableEditor<float, FloatVariable>
    {
        protected override float GetValue()
        {
            return _variableValue.floatValue;
        }
    }
}