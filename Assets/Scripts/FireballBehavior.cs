using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehavior : MonoBehaviour
{
    public float speed = 5f; // Velocidad inicial de la bola de fuego
    public float maxScale = 3f; // Tamaño máximo al que crecerá
    public float growthRate = 1f; // Velocidad de crecimiento
    public float lifetime = 5f; // Duración de la bola de fuego antes de destruirse

    private Vector3 initialScale;

    void Start()
    {
        // Guardar el tamaño inicial
        initialScale = transform.localScale;

        // Destruir la bola de fuego después de un tiempo
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Mover la bola de fuego hacia adelante
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Incrementar el tamaño de la bola de fuego
        if (transform.localScale.x < maxScale)
        {
            transform.localScale += Vector3.one * growthRate * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Aquí puedes añadir lógica para cuando colisione con enemigos
        if (collision.CompareTag("Enemy"))
        {
            // Por ejemplo, dañar al enemigo
            Destroy(gameObject); // Destruir la bola de fuego al colisionar
        }
    }
}

