using SaveSystem;
using UnityEngine;
using Utils;

namespace SceneMechanics
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BrittleSoil : IdentifiableObject, IDataPersistence
    {
        private bool _eventActivated;
        
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