using UnityEngine;

namespace Utils
{
    public class ScriptTest : MonoBehaviour
    {
        [ContextMenu("Input Test")]
        public void InputTest()
        {
            Debug.Log($"Player Actions Enabled: {InputManager.Instance.PlayerActions.enabled}");
        }
    }
}