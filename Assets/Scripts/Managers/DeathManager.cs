using GlobalVariables;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private IntReference _playerManna;
    
    [Space(10)]
    [SerializeField] private IntReference _playerHealth;
    [SerializeField] private IntReference _playerMaxHealth;
    
    [Space(10)]
    [SerializeField] private IntReference _playerPotions;
    [SerializeField] private IntReference _playerMaxPotions;
    
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        _playerManna.Value = 0;
        _playerHealth.Value = _playerMaxHealth;
        _playerPotions.Value = _playerMaxPotions;
    }
}