using UnityEngine;

public class FogEffect : MonoBehaviour
{
    [Header("Scale Effect")]
    [Tooltip("Velocidad del cambio de escala.")]
    public float scaleSpeed = 1.0f;

    [Tooltip("Escala máxima.")]
    public float maxScale = 1.5f;

    private Vector3 initialScale;

    [Header("Movement Effect")]
    [Tooltip("Velocidad del movimiento horizontal.")]
    public float moveSpeed = 1.0f;

    [Tooltip("Rango máximo del movimiento horizontal.")]
    public float maxHorizontalRange = 2.0f;

    private float initialX;
    private bool movingRight = true;

    private void Start()
    {
        initialScale = transform.localScale;
        initialX = transform.position.x;
    }

    private void Update()
    {
        HandleScaling();
        HandleHorizontalMovement();
    }

    private void HandleScaling()
    {
        float scaleFactor = Mathf.Sin(Time.time * scaleSpeed) * 0.5f + 0.5f; // Valor entre 0 y 1
        float newScale = Mathf.Lerp(initialScale.x, maxScale, scaleFactor);
        transform.localScale = new Vector3(newScale, newScale, initialScale.z);
    }

    private void HandleHorizontalMovement()
    {
        float movement = moveSpeed * Time.deltaTime;

        if (movingRight)
        {
            transform.position += new Vector3(movement, 0, 0);
            if (transform.position.x > initialX + maxHorizontalRange)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position -= new Vector3(movement, 0, 0);
            if (transform.position.x < initialX - maxHorizontalRange)
            {
                movingRight = true;
            }
        }
    }
}
