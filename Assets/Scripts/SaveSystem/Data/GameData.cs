using System.Collections.Generic;

namespace SaveSystem
{
    [System.Serializable]
    public class GameData
    {
        public List<int> abilitiesUnlocked = new List<int>();
        public GlobalVariables variables = new GlobalVariables();
        public string lastSaveScene;

        public SerializableDictionary<string, bool> upgradeItemsCollected = new SerializableDictionary<string, bool>();
        public SerializableDictionary<string, bool> deadEnemies = new SerializableDictionary<string, bool>();
        public SerializableDictionary<string, bool> sceneEvents = new SerializableDictionary<string, bool>();
    }

    [System.Serializable]
    public class GlobalVariables
    {
        public int maxHealth = 100;
        public int maxPotions = 3;

        public int attackItemAmount = 0;
        public int healthItemAmount = 0;
    }
}