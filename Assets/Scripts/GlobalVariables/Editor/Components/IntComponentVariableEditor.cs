using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(IntComponentVariable))]
    public class IntComponentVariableEditor : BaseComponentVariableEditor<int, IntComponentVariable>
    {
        protected override int GetValue()
        {
            return _variableValue.intValue;
        }
    }
}