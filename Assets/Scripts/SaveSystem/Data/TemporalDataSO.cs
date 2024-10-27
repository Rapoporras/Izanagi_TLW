using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    [CreateAssetMenu(fileName = "New TemporalData", menuName = "Save System/Temporal Data")]
    public class TemporalDataSO : ScriptableObject
    {
        public List<string> DeadEnemies = new List<string>();

        public void Clear()
        {
            Debug.Log("Clear temporal data");
            DeadEnemies.Clear();
        }
    }
}