using Generation.Dungeon;
using UnityEngine;

namespace Systems.Dungeon.Data
{
    public class DungeonRoom
    {
        public Vector2Int Position { get; set; }
        public RoomType Type { get; set; }
    }
}