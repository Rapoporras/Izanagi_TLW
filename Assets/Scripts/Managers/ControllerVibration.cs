using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVibration : MonoBehaviour
{
    // Instancia estática del Singleton
    public static ControllerVibration Instance { get; private set; }

    private Gamepad gamepad;

    private Coroutine heartbeatCoroutine; // Para controlar la corutina del latido

    // Asegurarse de que solo haya una instancia del Singleton
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
    void OnDisable()
    {
        // Detenemos cualquier vibración activa al deshabilitar el objeto
        StopAllCoroutines();
        StopVibration();
    }

    void OnDestroy()
    {
        // Aseguramos que no quede vibración activa al destruir el objeto
        StopAllCoroutines();
        StopVibration();
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

    // Método para vibración de latidos (corazón)
    public void StartHeartbeatVibration(float intensity, float beatDuration, float pauseBetweenBeats)
    {
        if (gamepad != null)
        {
            // Si ya hay un latido activo, lo detenemos antes de iniciar uno nuevo
            if (heartbeatCoroutine != null)
            {
                StopCoroutine(heartbeatCoroutine);
                StopVibration();
            }

            heartbeatCoroutine = StartCoroutine(HeartbeatVibration(intensity, beatDuration, pauseBetweenBeats));
        }
    }

    private System.Collections.IEnumerator HeartbeatVibration(float intensity, float beatDuration, float pauseBetweenBeats)
    {
        while (true) // Esto hará que el latido se repita indefinidamente
        {
            // Primer golpe
            gamepad.SetMotorSpeeds(intensity, intensity);
            yield return new WaitForSeconds(beatDuration);

            // Pausa breve
            gamepad.SetMotorSpeeds(0f, 0f);
            yield return new WaitForSeconds(pauseBetweenBeats * 0.5f);

            // Segundo golpe
            gamepad.SetMotorSpeeds(intensity, intensity);
            yield return new WaitForSeconds(beatDuration);

            // Pausa larga antes de repetir el ciclo
            gamepad.SetMotorSpeeds(0f, 0f);
            yield return new WaitForSeconds(pauseBetweenBeats);
        }
    }

    // Detener el latido y todas las vibraciones
    public void StopHeartbeatVibration()
    {
        if (heartbeatCoroutine != null)
        {
            StopCoroutine(heartbeatCoroutine);
            heartbeatCoroutine = null;
        }
        StopVibration();
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