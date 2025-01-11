using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    
    [Header("Reference to the player")]
    public GameObject player;
    
    [Header("Audio Settings")]
    [SerializeField] private float soundRadius;
    [SerializeField] private float fullVolumeRadius;
    [SerializeField, Range(0, 1)] private float maxVolume = 1f;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    
    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        _audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (player == null) return;
        
        float distance = Vector3.Distance(player.transform.position, transform.position);
        
        if (distance <= fullVolumeRadius)
        {
            _audioSource.volume = maxVolume;
        } else if (distance <= soundRadius)
        {
            _audioSource.volume = (1f - ((distance - fullVolumeRadius) / (soundRadius - fullVolumeRadius))) * maxVolume;
        }
        else
        {
            _audioSource.volume = 0;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, soundRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, fullVolumeRadius);
    }
}
