using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioPlay : MonoBehaviour
{
    
    private AudioSource _audioSource;

    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    [Header("Audio Settings")]
    [SerializeField] private bool useRandomVolume;
    [SerializeField, Range(0, 1)] private float minVolume = 1f;
    [SerializeField, Range(0, 1)] private float maxVolume = 1f;
    
    [SerializeField] private bool useRandomPitch;
    [SerializeField, Range(-3, 3)] private float minPitch = 1f;
    [SerializeField, Range(-3, 3)] private float maxPitch = 1f;
    
    [SerializeField] private bool loop;
    [SerializeField] private float minDelayBetweenPlays = 0.5f;
    [SerializeField] private float maxDelayBetweenPlays = 1f;

    private float _randomTimer;
    private float _randomVolume;
    private float _randomPitch;
    private bool _timerTickingDown;
    
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (_audioSource.isPlaying) return;

        if (_timerTickingDown)
        {
            _randomTimer -= Time.deltaTime;
        }
        else
        {
            _audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
            _randomTimer = Random.Range(minDelayBetweenPlays, maxDelayBetweenPlays);
            _randomVolume = Random.Range(minVolume, maxVolume);
            _randomPitch = Random.Range(minPitch, maxPitch);
            _timerTickingDown = true;
        }

        if (_randomTimer <= 0 || loop)
        {
            if (useRandomVolume) _audioSource.volume = _randomVolume;
            if (useRandomPitch) _audioSource.pitch = _randomPitch;
            _audioSource.Play();
            _timerTickingDown = false;
        }
    }
}
