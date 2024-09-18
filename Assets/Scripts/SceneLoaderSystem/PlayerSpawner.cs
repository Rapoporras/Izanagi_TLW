using Cinemachine;
using UnityEngine;

namespace SceneLoaderSystem
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerPathSO _playerPath;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private CinemachineVirtualCamera _followCamera;
        [SerializeField] private GameObject _playerParent;

        public void InstantiatePlayerOnLevel()
        {
            GameObject player = GetPlayer();
            Transform entrance = GetLevelEntrance(_playerPath.levelEntrance);

            player.transform.position = entrance.transform.position;
            player.transform.parent = _playerParent.transform;
            _followCamera.Follow = player.transform;

            _playerPath.levelEntrance = null;
        }

        private GameObject GetPlayer()
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (!playerObject)
            {
                playerObject = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
            }

            return playerObject;
        }

        private Transform GetLevelEntrance(LevelEntranceSO playerEntrance)
        {
            if (!playerEntrance)
            {
                // no path for player, instantiate it at default position
                return transform.GetChild(0).transform;
            }

            var levelEntrances = GameObject.FindObjectsOfType<LevelEntrance>();
            
            foreach (var levelEntrance in levelEntrances)
            {
                if (levelEntrance.entrance == playerEntrance)
                {
                    return levelEntrance.gameObject.transform;
                }
            }

            return transform.GetChild(0).transform;
        }
    }
}