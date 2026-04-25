using System;
using System.Linq;
using Generation;
using Generation.Dungeon;
using Systems.Dungeon.Data;
using UnityEngine;

namespace Systems.Dungeon
{
    public interface IDungeonManager
    {
        void GenerateDungeon();

        Data.Dungeon Dungeon { get;  }
        
        event Action<Data.Dungeon> OnDungeonGenerated;
    }

    public class DungeonManager : MonoBehaviour, IDungeonManager
    {
        [SerializeField] private MultistepGeneration generation;
        
        public Data.Dungeon Dungeon { get; private set; }
        public event Action<Data.Dungeon> OnDungeonGenerated;

        public void GenerateDungeon()
        {
            var generationResult = generation.Generate<DungeonGenerationContext>();

            var rooms = generationResult.Rooms.Select(x => new DungeonRoom()
                {
                    Position = x,
                    Type = generationResult.RoomTypes[x]
                }
            );
            
            var dungeon = new Data.Dungeon(
                rooms: rooms,
                connections: generationResult.Connections
            );
            
            Dungeon = dungeon;
            
            OnDungeonGenerated?.Invoke(dungeon);
        }

    }
}