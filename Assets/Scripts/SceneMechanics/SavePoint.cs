using InteractionSystem;
using SaveSystem;
using SceneLoaderSystem;
using UnityEngine;

namespace SceneMechanics
{
    public class SavePoint : MonoBehaviour, IInteractable
    {
        [Header("Dependencies")]
        [SerializeField] private SceneSO _currentScene;
        
        [Header("UI")]
        [SerializeField] private GameObject _interactUIText;

        public void Interact(Interactor interactor)
        {
            DataPersistenceManager.Instance.gameData.lastSaveScene = _currentScene.sceneName;
            TemporalDataManager.Instance.temporalData.EnemiesStatus.Clear();
            
            DataPersistenceManager.Instance.SaveGame();
            
            Debug.Log("Saving game . . .");
        }

        public void ShowInteractionUI(bool showUI)
        {
            _interactUIText.SetActive(showUI);
        }
    }
}