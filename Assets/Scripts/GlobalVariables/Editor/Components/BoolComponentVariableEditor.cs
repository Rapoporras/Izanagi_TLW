using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(BoolComponentVariable))]
    public class BoolComponentVariableEditor : BaseComponentVariableEditor<bool, BoolComponentVariable>
    {
        protected override bool GetValue()
        {
            return _variableValue.boolValue;
        }
    }
}