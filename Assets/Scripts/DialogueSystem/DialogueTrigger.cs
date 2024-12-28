using GameEvents;
using InteractionSystem;
using UnityEngine;
using Utils.CustomLogs;

namespace DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueInfo _dialogueInfo;
        [Space(10)]
        
        [Header("UI")]
        [SerializeField] private GameObject _dialogueBubbleImage;

        [Header("Game Events")]
        [SerializeField] private DialogueInfoEvent _onDialogueTriggerEvent;

        private void Awake()
        {
            if (_dialogueBubbleImage)
                _dialogueBubbleImage.SetActive(false);
        }

        public void Interact(Interactor interactor)
        {
            if (!_dialogueInfo.InkJSON)
            {
                LogManager.LogWarning("There is no ink Json", FeatureType.Dialogue);
                return;
            }
            
            if (_onDialogueTriggerEvent)
                _onDialogueTriggerEvent.Raise(_dialogueInfo);
        }

        public void ShowInteractionUI(bool showUI)
        {
            if (_dialogueBubbleImage)
                _dialogueBubbleImage.SetActive(showUI);
        }
    }
}