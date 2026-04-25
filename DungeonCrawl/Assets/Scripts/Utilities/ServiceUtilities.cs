using UnityEngine;

namespace DefaultNamespace.Utilities
{
    public static class ServiceUtilities
    {
        public static T GetService<T>() where T : Object
        {
            var service = Object.FindFirstObjectByType<T>();
            
            if (service == null)
            {
                Debug.LogError($"Service of type {typeof(T)} not found in the scene.");
            }
            
            return service;
        }
    }
}