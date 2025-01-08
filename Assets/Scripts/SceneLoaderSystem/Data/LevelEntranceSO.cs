using UnityEngine;

namespace SceneLoaderSystem
{
    [CreateAssetMenu(fileName = "NewLevelEntrance", menuName = "Scene Loader/Level Entrance")]
    public class LevelEntranceSO : ScriptableObject
    {
        public bool setPlayerFacingRight;
        [Tooltip("Set to true if this level entrance corresponds to a save statue")]
        public bool respawnFromDeath;
    }
}