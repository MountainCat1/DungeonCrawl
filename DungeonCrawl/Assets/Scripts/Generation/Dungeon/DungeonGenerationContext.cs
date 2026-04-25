using System.Collections.Generic;

namespace Generation.Dungeon
{
    public class DungeonGenerationContext : IGenerationContext
    {
        public HashSet<RoomGenerationData> Rooms = new();
        public Dictionary<RoomGenerationData, ICollection<RoomGenerationData>> Connections = new();
        
        public IGenerationContext CreateDefault()
        {
            return new DungeonGenerationContext
            {
            };
        }
    }
}