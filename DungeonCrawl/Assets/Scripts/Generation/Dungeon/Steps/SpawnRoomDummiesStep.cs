using Generation.Dungeon;
using UnityEngine;

namespace Generation.Steps
{
    public class SpawnRoomDummiesStep : DungeonGenerationStep
    {
        [SerializeField] private GameObject dummyPrefab;
        
        
        
        
        public override void Process(DungeonGenerationContext ctx)
        {
            foreach (var roomPositions in ctx.Rooms)
            {
                Instantiate(dummyPrefab, new Vector3(roomPositions.x, roomPositions.y), Quaternion.identity, transform);
            }
        }
    }
}