using UnityEngine;

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
    }
    
    private void OnDisable()
    {
        PlayerActions.Disable();
    }
}