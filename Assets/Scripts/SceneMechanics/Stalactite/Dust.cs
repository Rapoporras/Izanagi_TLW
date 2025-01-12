using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

public class Dust : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ParticleSystem _particleSystem1;
    [SerializeField] private ParticleSystem _particleSystem2;

    private void Awake()
    {
        _particleSystem1.Stop();
        _particleSystem2.Stop();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _particleSystem1.Play();
                _particleSystem2.Play();
            }
        }
}

