﻿using UnityEngine;

namespace Utils.CustomLogs
{
    public enum FeatureType
    {
        Undefined, Dialogue, Player, SaveSystem, InputSystem, CameraSystem, Abilities
    }
    
    [System.Serializable]
    public class FeatureLog
    {
        public FeatureType feature;
        public Color customColor;
        public bool enabled;
    }
}