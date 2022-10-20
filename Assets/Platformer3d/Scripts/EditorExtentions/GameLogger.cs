using UnityEngine;

namespace Platformer3d.EditorExtentions
{
    public static class GameLogger
    {
        public enum LogType : byte
        {
            Message = 0,
            Warning = 1,
            Error = 2,
            Fatal = 3,
        }

        public static void AddMessage(string message, LogType type = LogType.Message)
        {
#if UNITY_EDITOR
            switch (type)
            {
                case LogType.Message:
                    Debug.Log(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Fatal:
                    Debug.LogError(message);
                    Debug.Break();
                    break;
            }
#endif
        }
    }
}
