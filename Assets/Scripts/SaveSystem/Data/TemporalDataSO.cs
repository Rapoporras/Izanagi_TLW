using System.Collections.Generic;
using UnityEngine;
using Utils.CustomLogs;

namespace SaveSystem
{
    [CreateAssetMenu(fileName = "New TemporalData", menuName = "Save System/Temporal Data")]
    public class TemporalDataSO : ScriptableObject
    {
        public List<string> deadEnemies = new List<string>();

        public void Clear()
        {
            LogManager.Log("Clear temporal data", FeatureType.SaveSystem);
            deadEnemies.Clear();
        }
    }
}