using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarolEye : MonoBehaviour
{

    private Transform player;  // Referencia al jugador
    public float rangoDeteccion = 5f;  // Rango de detección del farol
    public float velocidadSeguir = 2f;  // Velocidad a la que sigue al jugador
    public float limiteIzq = -5f;  // Límite izquierdo del farol
    public float limiteDer = 5f;   // Límite derecho del farol

    private bool jugadorDetectado = false;

    private void Update()
    {
        if (jugadorDetectado && player != null)
        {
            // Obtener la posición del jugador
            Vector3 playerPos = player.position;

            // Calcular la dirección del jugador
            float direccion = playerPos.x > transform.position.x ? -1 : 1;

            // // Mover el ojo un paso pequeño en la dirección del jugador
            float nuevaPosX = transform.position.x * direccion * velocidadSeguir;

            // // Limitar el movimiento del ojo dentro de los límites
            nuevaPosX = Mathf.Clamp(nuevaPosX, limiteIzq, limiteDer);

            // // Actualizar la posición del ojo (en el eje X)
            transform.localPosition = new Vector3(nuevaPosX, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            // Si el jugador no está en el rango, el ojo se queda en su posición inicial
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
    }
    // Detectar la entrada del jugador en el rango
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Asegúrate de que el jugador tenga el tag "Player"
        {
            // Obtener la referencia al jugador cuando entra en el trigger
            player = other.transform;
            jugadorDetectado = true;
        }
    }

    // Detectar cuando el jugador sale del rango
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDetectado = false;
            player = null;  // Eliminar la referencia cuando el jugador salga
        }
    }

    // Visualiza el área de detección (Opcional, solo para desarrollo)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
