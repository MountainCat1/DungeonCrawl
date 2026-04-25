using System.Collections.Generic;
using UnityEngine;

namespace Generation.Dungeon
{
    public class DungeonGenerationContext : IGenerationContext
    {
        public HashSet<Vector2Int> Rooms = new();
        public Dictionary<Vector2Int, RoomType> RoomTypes = new();
        public Dictionary<Vector2Int, HashSet<Vector2Int>> Connections = new();
        
        public IGenerationContext CreateDefault()
        {
            return new DungeonGenerationContext
            {
            };
        }
    }
}