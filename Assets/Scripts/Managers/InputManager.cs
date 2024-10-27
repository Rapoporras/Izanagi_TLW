using UnityEngine;
using Utils.CustomLogs;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    public PlayerInputActions.PlayerActions PlayerActions => PlayerInputActions.Player;
    public PlayerInputActions.UIActions UIActions => PlayerInputActions.UI;
    
    private PlayerInputActions _playerInputActions;
    private PlayerInputActions PlayerInputActions
    {
        get
        {
            if (_playerInputActions == null)
                _playerInputActions = new PlayerInputActions();
            return _playerInputActions;
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        PlayerActions.Enable();
        UIActions.Enable();
    }
    
    private void OnDisable()
    {
        PlayerActions.Disable();
        UIActions.Disable();
    }

    public void EnablePlayerActions()
    {
        LogManager.Log("Enable player actions", FeatureType.InputSystem);
        PlayerActions.Enable();
    }
    
    public void DisablePlayerActions()
    {
        LogManager.Log("Disable player actions", FeatureType.InputSystem);
        PlayerActions.Disable();
    }
    
    public void EnableUIActions()
    {
        LogManager.Log("Enable UI actions", FeatureType.InputSystem);
        UIActions.Enable();
    }
    
    public void DisableUIActions()
    {
        LogManager.Log("Disable UI actions", FeatureType.InputSystem);
        UIActions.Disable();
    }
}