using System.Collections.Generic;

using UnityEngine;

namespace Utilities
{
    public static class CollisionUtility
    {
        public static List<T> GetObjectsInRadius<T>(Vector2 position, float radius, T[] ignore = null)
        {
            List<T> hits = new();
            HashSet<T> ignoreSet = ignore != null ? new(ignore) : new();

            var results = new List<Collider2D>();

            ContactFilter2D filter = new();
            filter.useTriggers = true;
            filter.NoFilter();

            Physics2D.OverlapCircle(position, radius, filter, results);

            foreach (var hitCollider in results)
            {
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