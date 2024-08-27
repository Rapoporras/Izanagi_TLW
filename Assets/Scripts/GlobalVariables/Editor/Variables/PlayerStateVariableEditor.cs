using PlayerController.States;
using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(PlayerStateVariable))]
    public class PlayerStateVariableEditor : BaseVariableEditor<PlayerStates, PlayerStateVariable>
    {
        protected override PlayerStates GetValue()
        {
            return (PlayerStates) _variableValue.enumValueIndex;
        }
    }
}