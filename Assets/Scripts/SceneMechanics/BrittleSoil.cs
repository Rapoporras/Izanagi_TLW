using System;
using KrillAudio.Krilloud;
using SaveSystem;
using UnityEngine;
using Utils;

namespace SceneMechanics
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BrittleSoil : IdentifiableObject, IDataPersistence
    {
        private bool _eventActivated;

        private KLAudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<KLAudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_eventActivated)
            {
                DestroySoil();
            }
        }

        private void DestroySoil()
        {
            // add some effects here
            _eventActivated = true;
            gameObject.SetActive(false);
            _audioSource.Play();
        }

        public void LoadData(GameData data)
        {
            data.sceneEvents.TryGetValue(id, out _eventActivated);
            if (_eventActivated)
            {
                gameObject.SetActive(false);
            }
        }

        public void SaveData(ref GameData data)
        {
            if (data.sceneEvents.ContainsKey(id))
            {
                data.sceneEvents.Remove(id);
            }
            data.sceneEvents.Add(id, _eventActivated);
        }
    }
}