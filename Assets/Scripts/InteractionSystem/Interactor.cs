using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem
{
    public class Interactor : MonoBehaviour
    {
        private List<IInteractable> _interactables = new List<IInteractable>();

        private void OnEnable()
        {
            InputManager.Instance.PlayerActions.Interact.performed += Interact;
        }

        private void OnDisable()
        {
            InputManager.Instance.PlayerActions.Interact.performed -= Interact;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                _interactables.Add(interactable);
                interactable.ShowInteractionUI(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                _interactables.Remove(interactable);
                interactable.ShowInteractionUI(false);
            }
        }

        private void Interact(InputAction.CallbackContext context)
        {
            if (_interactables.Count == 0) return;

            _interactables[0].ShowInteractionUI(false); // hide UI after interaction
            _interactables[0].Interact(this);
        }

        private void OnValidate()
        {
            Transform interactionAreaTransform = transform.Find("Interaction Area");
            if (!interactionAreaTransform)
            {
                GameObject interactionArea = new GameObject("Interaction Area");
                interactionArea.transform.parent = transform;
                interactionArea.transform.localPosition = Vector3.zero;

                interactionArea.layer = LayerMask.NameToLayer("Interaction");
                BoxCollider2D trigger = interactionArea.AddComponent<BoxCollider2D>();
                trigger.isTrigger = true;
            }
        }
    }
}