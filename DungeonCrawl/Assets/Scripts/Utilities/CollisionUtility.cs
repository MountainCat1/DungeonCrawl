using System.Collections.Generic;

using UnityEngine;

namespace Utilities
{
    public static class CollisionUtility
    {
        public static bool IsObstacle(GameObject go)
        {
            return go.layer == LayerMask.NameToLayer("Obstacles");
        }
        
        public static bool IsWall(GameObject go)
        {
            return go.layer == LayerMask.NameToLayer("Walls");
        }

        public static int BlockingVisionLayerMask => LayerMask.GetMask("Obstacles", "Walls");
        public static int UnwalkableLayerMask => LayerMask.GetMask("Obstacles", "Walls");
        
        public static List<T> GetObjectsInRadius<T>(Vector2 position, float radius, T[] ignore = null)
        {
            List<T> hits = new();
            HashSet<T> ignoreSet = ignore != null ? new(ignore) : new();

            Collider2D[] results = new Collider2D[50];
            int size = Physics2D.OverlapCircleNonAlloc(position, radius, results);

            for (int i = 0; i < size; i++)
            {
                var hitCollider = results[i];
                var hit = hitCollider.GetComponent<T>();

                
                if (hit != null && !ignoreSet.Contains(hit))
                {
                    hits.Add(hit);
                }
            }

            return hits;
        }
    }
}