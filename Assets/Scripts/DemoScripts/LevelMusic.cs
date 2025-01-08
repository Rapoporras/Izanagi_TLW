using System;
using KrillAudio.Krilloud;
using UnityEngine;

namespace DemoScripts
{
    public class LevelMusic : MonoBehaviour
    {
        [KLChannel, SerializeField] private string _musicChannel;
        [SerializeField, Range(0, 1)] private float _volume;

        private void Start()
        {
            KLCenter.SetChannelVolume(_musicChannel, _volume);
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                KLCenter.SetChannelVolume(_musicChannel, _volume);
            }
        }
    }
}