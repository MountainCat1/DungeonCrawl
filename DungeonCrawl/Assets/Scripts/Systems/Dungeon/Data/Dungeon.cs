using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Dungeon.Data
{
    public class Dungeon
    {
        public IReadOnlyCollection<DungeonRoom> Rooms => _rooms;
        public IReadOnlyDictionary<Vector2Int, HashSet<Vector2Int>> Connections => _connections;


        private readonly Dictionary<Vector2Int, HashSet<Vector2Int>> _connections;
        private readonly List<DungeonRoom> _rooms;

        public Dungeon(IEnumerable<DungeonRoom> rooms, Dictionary<Vector2Int, HashSet<Vector2Int>> connections)
        {
            _rooms = new List<DungeonRoom>();
            _rooms.AddRange(rooms);
            
            _connections = connections;
        }
    }
}