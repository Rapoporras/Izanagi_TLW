using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    [CreateAssetMenu(fileName = "New TemporalData", menuName = "Save System/Temporal Data")]
    public class TemporalDataSO : ScriptableObject
    {
        /// <summary>
        /// <para><b>key</b> - Enemy id</para>
        /// <para><b>value</b> - Whether the enemy is dead (true) or alive (false)</para>
        /// </summary>
        public readonly Dictionary<string, bool> EnemiesStatus = new Dictionary<string, bool>();

        public void Clear()
        {
            EnemiesStatus.Clear();
        }
    }
}