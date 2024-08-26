using PlayerController.States;
using UnityEditor;

namespace GlobalVariables.Editor
{
    [CustomEditor(typeof(PlayerStateComponentVariable))]
    public class PlayerStateComponentVariableEditor : BaseComponentVariableEditor<PlayerStates, PlayerStateComponentVariable>
    {
        protected override PlayerStates GetValue()
        {
            return (PlayerStates) _variableValue.enumValueIndex;
        }
    }
}