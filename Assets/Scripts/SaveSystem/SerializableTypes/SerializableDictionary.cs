﻿using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();
        
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            if (keys.Count != values.Count)
            {
                Debug.LogError("Tried to deserialized a SerializableDictionary, but the amount of keys ("
                    + keys.Count + ") does not match the number of values (" + values.Count + ") which indicates" +
                    "that something went wrong.");
            }
            
            for (int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }
    }
}