using System.Collections.Generic;

namespace SaveSystem
{
    [System.Serializable]
    public class GameData
    {
        /*
         * fire ability unlocked
         * 1 -> fire
         * 2 -> ground
         * 3 -> air
         * 4 -> water
         */
        public List<int> abilitiesUnlocked = new List<int>() { 1 };
        public GlobalVariables variables = new GlobalVariables();
        public string lastSaveScene;
        // public LastSaveInfo lastSaveInfo;
        
        
        public SerializableDictionary<string, bool> upgradeItemsCollected = new SerializableDictionary<string, bool>();
        public SerializableDictionary<string, bool> sceneEvents = new SerializableDictionary<string, bool>();
    }

    [System.Serializable]
    public class GlobalVariables
    {
        public int maxHealth = 100;
        public int maxPotions = 3;

        public int attackItemAmount = 0;
        public int healthItemAmount = 0;

        public float attackMultiplier = 1f;
    }

    [System.Serializable]
    public class LastSaveInfo
    {
        public bool hasSaved;
        public string sceneName;
        public string statueId;
    }
}