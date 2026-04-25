using System;
using System.Collections.Generic;
using System.Linq;
using Generation;
using Generation.Dungeon;
using Systems.Dungeon.Data;
using UnityEngine;

namespace Systems.Dungeon
{
    public interface IDungeonManager
    {
        Data.Dungeon GenerateDungeon();

        Data.Dungeon Dungeon { get;  }
        
        event Action<Data.Dungeon> DungeonGenerated;
    }

    public class DungeonManager : MonoBehaviour, IDungeonManager
    {
        [SerializeField] private MultistepGeneration generation;
        
        public Data.Dungeon Dungeon { get; private set; }
        public event Action<Data.Dungeon> DungeonGenerated;

        public Data.Dungeon GenerateDungeon()
        {
            var generationResult = generation.Generate<DungeonGenerationContext>();

            // map generation rooms -> runtime rooms
            var roomMap = generationResult.Rooms.ToDictionary(
                x => x,
                x => new DungeonRoom
                {
                    Position = x.Position,
                    Type = x.Type
                }
            );

            // build neighbours directly
            foreach (var kvp in generationResult.Connections)
            {
                var from = roomMap[kvp.Key];

                foreach (var toGen in kvp.Value)
                {
                    var to = roomMap[toGen];

                    from.Neighbours.Add(to);
                    to.Neighbours.Add(from); // ensure bidirectional (safe even if duplicated)
                }
            }

            var dungeon = new Data.Dungeon(
                rooms: roomMap.Values
            );

            Dungeon = dungeon;

            DungeonGenerated?.Invoke(dungeon);

            return Dungeon;
        }
    }
}