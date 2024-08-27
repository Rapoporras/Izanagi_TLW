using GlobalVariables;
using UnityEngine;
using InteractionSystem;

namespace Items
{
    public class UpgradeItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private IntReference _currentItemAmount;
        
        public void Interact(Interactor interactor)
        {
            Debug.Log("item obtained");
            _currentItemAmount.Value += 1;
            // Destroy(gameObject);
        }
    }
}