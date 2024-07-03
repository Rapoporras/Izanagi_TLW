using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(StringVariable))]
    public class StringIntVariableEditor : BaseVariableEditor<string, StringVariable>
    {
        protected override string GetValue()
        {
            return _variableValue.stringValue;
        }
    }
}