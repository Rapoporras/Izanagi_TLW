using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebaat : MonoBehaviour
{
    public GameObject fireballPrefab; // Prefab de la bola de fuego
    public Transform spawnPoint; // Punto de origen del ataque

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Cambiar la tecla según prefieras
        {
            Fire();
        }
    }

    void Fire()
    {
        // Crear la bola de fuego en el punto de origen
        Instantiate(fireballPrefab, spawnPoint.position, Quaternion.identity);
    }
}
