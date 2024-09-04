using GlobalVariables;
using UnityEngine;
using InteractionSystem;
using UnityEngine.Serialization;

namespace Items
{
    public class UpgradeItem : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField] private IntReference _currentItemAmount;
        [SerializeField, Min(0)] private int _amountToAdd = 1;

        [FormerlySerializedAs("_interactText")]
        [Header("UI")]
        [SerializeField] private GameObject _interactUIText;

        private bool _used;
        
        public void Interact(Interactor interactor)
        {
            if (_used) return;
            
            Debug.Log("item obtained");
            _currentItemAmount.Value += _amountToAdd;
            
            _used = true;
            
            // add here some animation
            Destroy(gameObject);
        }

        public void ShowInteractionUI(bool showUI)
        {
            if (_used) return;
            _interactUIText.SetActive(showUI);
        }
    }
}