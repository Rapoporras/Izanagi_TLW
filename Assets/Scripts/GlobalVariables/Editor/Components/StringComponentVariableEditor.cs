using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(StringComponentVariable))]
    public class StringComponentVariableEditor : BaseComponentVariableEditor<string, StringComponentVariable>
    {
        protected override string GetValue()
        {
            return _variableValue.stringValue;
        }
    }
}