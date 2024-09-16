using GlobalVariables;
using UnityEngine;
using InteractionSystem;
using SaveSystem;
using UnityEngine.Serialization;

namespace Items
{
    public class UpgradeItem : MonoBehaviour, IInteractable, IDataPersistence
    {
        [SerializeField, ReadOnly] private string id;
        [Space(10)]
        
        [Header("Settings")]
        [SerializeField] private IntReference _currentItemAmount;
        [SerializeField, Min(0)] private int _amountToAdd = 1;

        [FormerlySerializedAs("_interactText")]
        [Header("UI")]
        [SerializeField] private GameObject _interactUIText;

        private bool _collected;
        
        public void Interact(Interactor interactor)
        {
            if (_collected) return;
            
            Debug.Log("item obtained");
            _currentItemAmount.Value += _amountToAdd;
            
            _collected = true;
            
            // add here some animation
            Destroy(gameObject);
        }

        public void ShowInteractionUI(bool showUI)
        {
            if (_collected) return;
            _interactUIText.SetActive(showUI);
        }

        [ContextMenu("Generate guid for id")]
        private void GenerateGuid()
        {
            id = System.Guid.NewGuid().ToString();
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
                GenerateGuid();
        }

        public void LoadData(GameData data)
        {
            data.upgradeItemsCollected.TryGetValue(id, out _collected);
            if (_collected)
            {
                gameObject.SetActive(false);
            }
        }

        public void SaveData(ref GameData data)
        {
            if (data.upgradeItemsCollected.ContainsKey(id))
            {
                data.upgradeItemsCollected.Remove(id);
            }
            data.upgradeItemsCollected.Add(id, _collected);
        }
    }
}