using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVibration : MonoBehaviour
{
    // Instancia estática del Singleton
    public static ControllerVibration Instance { get; private set; }

    private Gamepad gamepad;

    // Asegurarse de que solo haya una instancia del singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        gamepad = Gamepad.current;
    }

    // Método para vibración instantánea
    public void TriggerInstantVibration(float leftMotorIntensity, float rightMotorIntensity, float duration)
    {
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(leftMotorIntensity, rightMotorIntensity);
            Invoke("StopVibration", duration); // Detiene la vibración después de la duración
        }
    }

    // Método para vibración progresiva: aumenta la intensidad de la vibración
    public void TriggerProgressiveVibration(float maxIntensity, float duration)
    {
        if (gamepad != null)
        {
            StartCoroutine(VibrationProgression(maxIntensity, duration));  // Inicia la progresión de vibración
        }
    }

    // Corutina para controlar la vibración gradual
    private System.Collections.IEnumerator VibrationProgression(float maxIntensity, float duration)
    {
        float elapsedTime = 0f;

        // Aumenta la intensidad gradualmente
        while (elapsedTime < duration)
        {
            float intensity = Mathf.Lerp(0f, maxIntensity, elapsedTime / duration);  // Interpolación de la intensidad
            gamepad.SetMotorSpeeds(intensity, intensity);  // Aplica la misma intensidad a ambos motores

            elapsedTime += Time.deltaTime;  // Incrementa el tiempo
            yield return null;  // Espera el siguiente frame
        }

        // Asegúrate de detener la vibración después de que termine la duración
        gamepad.SetMotorSpeeds(0f, 0f);
    }

    // Método para detener la vibración inmediatamente
    public void StopVibration()
    {
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }
}
