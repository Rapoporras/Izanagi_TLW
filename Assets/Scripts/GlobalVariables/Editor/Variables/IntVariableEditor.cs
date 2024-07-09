using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(IntVariable))]
    public class IntVariableEditor : BaseVariableEditor<int, IntVariable>
    {
        protected override int GetValue()
        {
            return _variableValue.intValue;
        }
    }
}