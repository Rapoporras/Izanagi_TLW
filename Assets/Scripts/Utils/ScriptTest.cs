using UnityEngine;
using Utils.CustomLogs;

namespace Utils
{
    public class ScriptTest : MonoBehaviour
    {
        [ContextMenu("Input Test")]
        public void InputTest()
        {
            LogManager.Log($"Player Actions Enabled: {InputManager.Instance.PlayerActions.enabled}", FeatureType.InputSystem);
            LogManager.LogWarning("Warning", FeatureType.Undefined);
            LogManager.LogError("Error", FeatureType.Undefined);
        }
    }
}