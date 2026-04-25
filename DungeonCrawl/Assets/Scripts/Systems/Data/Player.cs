using System;
using Systems.Dungeon.Data;

namespace DefaultNamespace.Systems.Data
{
    public class PlayerData
    {
        public event Action Changed;
        
        public DungeonRoom CurrentRoom { get; private set; }

        public PlayerData(DungeonRoom initialRoom)
        {
            CurrentRoom = initialRoom;
        }

        public void MoveToRoom(DungeonRoom dungeonRoom)
        {
            CurrentRoom = dungeonRoom;
            
            Changed?.Invoke();
        }
    }
}