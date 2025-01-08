using GameEvents;
using GlobalVariables;
using UnityEngine;
using InteractionSystem;
using SaveSystem;
using Utils;

namespace Items
{
    public class UpgradeItem : IdentifiableObject, IInteractable, IDataPersistence
    {
        [Header("Settings")]
        [SerializeField] private IntReference _currentItemAmount;
        [SerializeField, Min(0)] private int _amountToAdd = 1;

        [Header("UI")]
        [SerializeField] private GameObject _interactUIText;

        [Header("Events")]
        [SerializeField] private IntEvent _onItemCollected;

        private bool _collected;
        
        public void Interact(Interactor interactor)
        {
            if (_collected) return;
            
            Debug.Log("item obtained");
            _currentItemAmount.Value += _amountToAdd;
            
            _collected = true;
            
            if (_onItemCollected)
                _onItemCollected.Raise(_currentItemAmount.Value);
            
            // add here some animation
            gameObject.SetActive(false);
        }

        public void ShowInteractionUI(bool showUI)
        {
            if (_collected) return;
            _interactUIText.SetActive(showUI);
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