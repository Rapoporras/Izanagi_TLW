﻿using Cinemachine;
using GlobalVariables;
using PlayerController;
using UnityEngine;

namespace SceneLoaderSystem
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerPathSO _playerPath;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private CinemachineVirtualCamera[] _virtualCameras;
        [SerializeField] private GameObject _playerParent;

        [Header("Variables")]
        [SerializeField] private IntReference _playerHealth;
        [SerializeField] private IntReference _playerMaxHealth;
        [Space(5)]
        [SerializeField] private IntReference _playerPotions;
        [SerializeField] private IntReference _playerMaxPotions;
        [Space(5)]
        [SerializeField] private IntReference _playerManna;

        public void InstantiatePlayerOnLevel()
        {
            GameObject player = GetPlayer();
            Transform entrance = GetLevelEntrance(_playerPath.levelEntrance);

            player.transform.position = entrance.transform.position;
            player.transform.parent = _playerParent.transform;
            
            if (player.TryGetComponent(out PlayerMovement movement))
            {
                movement.SetCameraFollowObject();
                foreach (var virtualCamera in _virtualCameras)
                {
                    virtualCamera.Follow = movement.CameraTarget;
                }
                bool isMovingRight = true;
                if (_playerPath.levelEntrance)
                    isMovingRight = _playerPath.levelEntrance.setPlayerFacingRight;
                
                movement.SetDirectionToFace(isMovingRight);
            }

            SetPlayerVariables();

            _playerPath.levelEntrance = null;

            BaseEnemy[] enemies = FindObjectsOfType<BaseEnemy>();
            foreach (var enemy in enemies)
            {
                enemy.player = player;
                enemy.SetUpBehaviourTree();
            }
            
            // all dependencies must be loaded at this point
            // there must be an InputManager
            InputManager.Instance.EnablePlayerActions();
        }

        private void SetPlayerVariables()
        {
            if (_playerPath.levelEntrance == null || _playerPath.levelEntrance.respawnFromDeath)
            {
                _playerHealth.Value = _playerMaxHealth;
                _playerPotions.Value = _playerMaxPotions;
                _playerManna.Value = 0;
            }
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

            var levelEntrances = FindObjectsOfType<LevelEntrance>();
            
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