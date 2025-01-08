using UnityEngine;
using Utils.CustomLogs;

public static class InputManager
{
    // public static InputManager Instance { get; private set; }
    
    public static PlayerInputActions.PlayerActions PlayerActions => PlayerInputActions.Player;
    public static PlayerInputActions.UIActions UIActions => PlayerInputActions.UI;
    
    private static PlayerInputActions _playerInputActions;
    private static PlayerInputActions PlayerInputActions
    {
        get
        {
            if (_playerInputActions == null)
            {
                _playerInputActions = new PlayerInputActions();
                _playerInputActions.Player.Enable();
                _playerInputActions.UI.Enable();
            }
            return _playerInputActions;
        }
    }

    public static void EnablePlayerActions()
    {
        LogManager.Log("Enable player actions", FeatureType.InputSystem);
        PlayerActions.Enable();
    }
    
    public static void DisablePlayerActions()
    {
        LogManager.Log("Disable player actions", FeatureType.InputSystem);
        PlayerActions.Disable();
    }
    
    public static void EnableUIActions()
    {
        LogManager.Log("Enable UI actions", FeatureType.InputSystem);
        UIActions.Enable();
    }
    
    public static void DisableUIActions()
    {
        LogManager.Log("Disable UI actions", FeatureType.InputSystem);
        UIActions.Disable();
    }
    
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _playerInputActions = null;
    }
#endif
}