using System.Collections.Generic;

namespace Systems.Dungeon.Data
{
    public class Dungeon
    {
        public IReadOnlyCollection<DungeonRoom> Rooms => _rooms;

        private readonly List<DungeonRoom> _rooms;

        public Dungeon(IEnumerable<DungeonRoom> rooms)
        {
            _rooms = new List<DungeonRoom>();
            _rooms.AddRange(rooms);
        }
    }
}