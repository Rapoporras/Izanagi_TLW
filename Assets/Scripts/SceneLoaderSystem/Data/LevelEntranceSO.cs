using UnityEngine;

namespace SceneLoaderSystem
{
    [CreateAssetMenu(fileName = "NewLevelEntrance", menuName = "Scene Loader/Level Entrance")]
    public class LevelEntranceSO : ScriptableObject
    {
        public bool setPlayerFacingRight;
        public bool respawnFromDeath;
    }
}