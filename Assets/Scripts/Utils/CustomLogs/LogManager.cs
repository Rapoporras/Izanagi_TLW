using UnityEngine;

namespace Utils.CustomLogs
{
    public static class LogManager
    {
        private const string LOGS_PATH = "Assets/ScriptableObjects/LogManager/LogData.asset";
        private static FeatureLogScriptable _logData;

        private static void LogDebugConfig()
        {
            _logData = (FeatureLogScriptable)UnityEditor.AssetDatabase.LoadAssetAtPath(LOGS_PATH,
                typeof(FeatureLogScriptable));

            if (_logData == null)
            {
                Debug.LogError($"Log Data was not found at path: {LOGS_PATH}");
            }
        }

        private static void LogConsole(string text, FeatureType feature)
        {
            var logData = _logData.GetLogData(feature);

            if (logData != null && logData.enabled)
            {
                var hexColor = ToHex(logData.customColor);
                string header = $"[<b><color={hexColor}>{feature}</color></b>]";
                Debug.Log($"{header} {text}");
            }
        }

        private static void LogWarningConsole(string text, FeatureType feature)
        {
            var logData = _logData.GetLogData(feature);

            if (logData != null && logData.enabled)
            {
                var hexColor = ToHex(logData.customColor);
                string header = $"[<b><color={hexColor}>{feature}</color></b>]";
                Debug.LogWarning($"{header} {text}");
            }
        }
        
        private static void LogErrorConsole(string text, FeatureType feature)
        {
            var logData = _logData.GetLogData(feature);

            if (logData != null && logData.enabled)
            {
                var hexColor = ToHex(logData.customColor);
                string header = $"[<b><color={hexColor}>{feature}</color></b>]";
                Debug.LogError($"{header} {text}");
            }
        }

        private static string ToHex(Color color)
        {
            var col = (Color32)color;
            return $"#{col.r:x2}{col.g:x2}{col.b:x2}";
        }
        
        public static void Log(string text, FeatureType feature)
        {
#if UNITY_EDITOR
            if (_logData == null)
                LogDebugConfig();
            
            LogConsole(text, feature);
#endif
        }

        public static void LogWarning(string text, FeatureType feature)
        {
#if UNITY_EDITOR
            if (_logData == null)
                LogDebugConfig();
            
            LogWarningConsole(text, feature);
#endif
        }
        
        public static void LogError(string text, FeatureType feature)
        {
#if UNITY_EDITOR
            if (_logData == null)
                LogDebugConfig();
            
            LogErrorConsole(text, feature);
#endif
        }
    }
}