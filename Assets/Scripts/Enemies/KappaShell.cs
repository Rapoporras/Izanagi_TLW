using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KappaShell : MonoBehaviour
{
    public float delay = 2.0f; // Tiempo en segundos antes de que el sprite aparezca
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Obtener el componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Asegurarse de que el sprite no sea visible inicialmente
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // Iniciar la corrutina para mostrar el sprite después del tiempo de espera
        StartCoroutine(ShowSpriteAfterDelay());
    }

    System.Collections.IEnumerator ShowSpriteAfterDelay()
    {
        // Esperar por el tiempo especificado
        yield return new WaitForSeconds(delay);

        // Hacer que el sprite sea visible
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }
}