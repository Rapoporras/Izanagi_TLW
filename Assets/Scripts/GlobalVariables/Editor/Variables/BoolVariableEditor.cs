using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(BoolVariable))]
    public class BoolVariableEditor : BaseVariableEditor<bool, BoolVariable>
    {
        protected override bool GetValue()
        {
            return _variableValue.boolValue;
        }
    }
}