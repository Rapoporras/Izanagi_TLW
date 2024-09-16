using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtns : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("WaterLevel");
    }

    public void Exit()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }
}
