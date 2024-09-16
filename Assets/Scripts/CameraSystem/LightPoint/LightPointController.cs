using UnityEngine;

public class LightPointController : MonoBehaviour
{
    public Transform player;
    public UnityEngine.Rendering.Universal.Light2D light2D;

    public float minIntensity = 0.5f;  // Intensidad minima de la luz
    public float maxIntensity = 1.5f;  // Intensidad maxima de la luz
    public float intensitySpeed = 1f;  // Velocidad del cambio de intensidad


    private float intensityOffset;
    private float sizeOffset;

    void Start()
    {
        // Inicializamos los offsets para variar los valores de forma suave
        intensityOffset = Random.Range(0f, 100f);
        sizeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position;
        }

        // Variar la intensidad de la luz de manera suave
        float intensityVariation = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * intensitySpeed + intensityOffset, 1));
        light2D.intensity = intensityVariation;
    }
}