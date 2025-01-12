using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GlobalVariables;
public class HeathVibration : MonoBehaviour
{
    [SerializeField] private IntReference _currentHealth;
    public float lowHealthThreshold = 20f; // Umbral para activar el latido

    private bool isHeartbeatActive = false;


    void Update()
    {
        if (_currentHealth < lowHealthThreshold && _currentHealth > 1f && !isHeartbeatActive)
        {
            // Activa el latido del corazón
            isHeartbeatActive = true;
            ControllerVibration.Instance.StartHeartbeatVibration(0.8f, 0.1f, 0.5f); // Intensidad 0.8, golpe de 0.1s, pausa entre latidos 0.5s
        }
        else if (_currentHealth > lowHealthThreshold && isHeartbeatActive)
        {
            // Detiene el latido del corazón
            isHeartbeatActive = false;
            ControllerVibration.Instance.StopHeartbeatVibration();
        }
    }
}