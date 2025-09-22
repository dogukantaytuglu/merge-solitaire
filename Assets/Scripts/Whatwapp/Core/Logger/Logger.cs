using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Whatwapp.Core.Logger
{
    public static class Logger
    {
        private const bool EnableLogsForBuilds = true;
        private static void DoLog(Action<string, Object> logFunction, string prefix, object myObj, params object[] msg)
        {
            if (Application.isEditor || EnableLogsForBuilds)
            {
                var name = GetObjectName(myObj, out var unityObject);
                logFunction($"{prefix}[{name}]: {String.Join("; ", msg)}\n ", unityObject ? unityObject : null);
            }
        }

        private static string GetObjectName(object myObj, out Object unityObject)
        {
            var name = "";
            unityObject = null;

            if (myObj != null)
            {
                var isUnityObject = myObj.GetType() == typeof(Object);

                if (isUnityObject)
                {
                    unityObject = (Object)myObj;
                    name = unityObject.name;
                }

                else
                {
                    var objName = myObj.ToString();
                    name = objName;
                }
            }

            else
            {
                name = "Null Object".Color("#FF4747");
            }

            name = name.ClampFromLeftUntil("(");

            return name.Color("#0077FF");
        }
        
        public static string ClampFromLeftUntil(this string value, string character)
        {
            if (!value.Contains(character))
            {
                return value;
            }
            
            var indexOfDot = value.IndexOf(character, StringComparison.Ordinal);
            value = value[..indexOfDot].Trim();
            return value;
        }
    
        public static void TempLog(this object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "", myObj, msg);
        }

        public static void Log(this object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "", myObj, msg);
        }

        public static void ConditionalLog(this object myObj, Func<bool> prerequisite, params object[] msg)
        {
            if (!prerequisite.Invoke()) return;
            DoLog(Debug.Log, "", myObj, msg);
        }
        
        public static void ConditionalLog(this object myObj, bool prerequisite, params object[] msg)
        {
            if (!prerequisite) return;
            DoLog(Debug.Log, "", myObj, msg);
        }

        public static void LogError(this object myObj, params object[] msg)
        {
            DoLog(Debug.LogError, "<!>".Color("#FF4747"), myObj, msg);
        }

        public static void LogWarning(this object myObj, params object[] msg)
        {
            DoLog(Debug.LogWarning, "⚠️".Color("yellow"), myObj, msg);
        }

        public static void LogSuccess(this object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "☻".Color("green"), myObj, msg);
        }
    
        private static string Color(this string myStr, string color)
        {
            return $"<color={color}>{myStr}</color>";
        }
    }
}