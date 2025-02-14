﻿using UnityEngine;

namespace SceneLoaderSystem
{
    [CreateAssetMenu(fileName = "PlayerPath", menuName = "Scene Loader/Player Path")]
    public class PlayerPathSO : ScriptableObject
    {
        public LevelEntranceSO levelEntrance;
        public LevelEntranceSO lastSavePoint;

        public void Clear()
        {
            levelEntrance = null;
            lastSavePoint = null;
        }
    }
}