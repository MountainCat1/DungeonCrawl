using System.Collections.Generic;
using Generation.Dungeon;
using UnityEngine;

namespace Generation.Steps
{
    public class GenerateRoomLayoutStep : DungeonGenerationStep
    {
        [SerializeField] private int targetRooms = 15;
        [SerializeField] private int maxNeighbors = 3;
        [SerializeField] private int seed;
        [SerializeField] private float extraConnectionChance = 0.15f;
        
        private static readonly Vector2Int[] Directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        public override void Process(DungeonGenerationContext ctx)
        {
            Random.InitState(seed == 0 ? System.Environment.TickCount : seed);

            ctx.Rooms.Clear();
            ctx.RoomTypes.Clear();

            var start = Vector2Int.zero;
            ctx.Rooms.Add(start);
            ctx.RoomTypes[start] = RoomType.Start;

            var frontier = new List<Vector2Int> { start };

            while (ctx.Rooms.Count < targetRooms && frontier.Count > 0)
            {
                var current = frontier[Random.Range(0, frontier.Count)];

                foreach (var dir in Shuffle(Directions))
                {
                    var next = current + dir;

                    if (ctx.Rooms.Contains(next))
                        continue;

                    if (CountNeighbors(ctx.Rooms, current) >= maxNeighbors)
                        continue;

                    if (CountNeighbors(ctx.Rooms, next) > 1)
                        continue; // avoid loops (Isaac mostly tree-like)

                    ctx.Rooms.Add(next);
                    ctx.RoomTypes[next] = RoomType.Normal;
                    frontier.Add(next);

                    Connect(ctx, current, next);

                    if (ctx.Rooms.Count >= targetRooms)
                        break;
                }

                // dead end, remove from frontier
                if (CountFreeNeighbors(ctx.Rooms, current) == 0)
                    frontier.Remove(current);
            }

            PlaceSpecialRooms(ctx);
            AddExtraConnections(ctx);
        }
        
        private void AddExtraConnections(DungeonGenerationContext ctx)
        {
            foreach (var room in ctx.Rooms)
            {
                foreach (var dir in Directions)
                {
                    var neighbor = room + dir;

                    if (!ctx.Rooms.Contains(neighbor))
                        continue;

                    // already connected
                    if (ctx.Connections.TryGetValue(room, out var conns) && conns.Contains(neighbor))
                        continue;

                    // prevent over-connecting
                    if (CountConnections(ctx, room) >= maxNeighbors)
                        continue;

                    if (CountConnections(ctx, neighbor) >= maxNeighbors)
                        continue;

                    // random chance to create loop
                    if (Random.value < extraConnectionChance)
                    {
                        Connect(ctx, room, neighbor);
                    }
                }
            }
        }

        private int CountConnections(DungeonGenerationContext ctx, Vector2Int room)
        {
            return ctx.Connections.TryGetValue(room, out var set) ? set.Count : 0;
        }

        private void Connect(DungeonGenerationContext ctx, Vector2Int a, Vector2Int b)
        {
            if (!ctx.Connections.TryGetValue(a, out var listA))
            {
                listA = new HashSet<Vector2Int>();
                ctx.Connections[a] = listA;
            }

            if (!ctx.Connections.TryGetValue(b, out var listB))
            {
                listB = new HashSet<Vector2Int>();
                ctx.Connections[b] = listB;
            }

            listA.Add(b);
            listB.Add(a);
        }
        
        private void PlaceSpecialRooms(DungeonGenerationContext ctx)
        {
            var deadEnds = new List<Vector2Int>();

            foreach (var room in ctx.Rooms)
            {
                if (CountNeighbors(ctx.Rooms, room) == 1 && ctx.RoomTypes[room] == RoomType.Normal)
                    deadEnds.Add(room);
            }

            if (deadEnds.Count == 0)
                return;

            var boss = deadEnds[Random.Range(0, deadEnds.Count)];
            ctx.RoomTypes[boss] = RoomType.Boss;
            deadEnds.Remove(boss);

            if (deadEnds.Count > 0)
            {
                var treasure = deadEnds[Random.Range(0, deadEnds.Count)];
                ctx.RoomTypes[treasure] = RoomType.Treasure;
                deadEnds.Remove(treasure);
            }

            if (deadEnds.Count > 0)
            {
                var shop = deadEnds[Random.Range(0, deadEnds.Count)];
                ctx.RoomTypes[shop] = RoomType.Shop;
            }
        }

        private int CountNeighbors(HashSet<Vector2Int> rooms, Vector2Int pos)
        {
            int count = 0;
            foreach (var dir in Directions)
            {
                if (rooms.Contains(pos + dir))
                    count++;
            }
            return count;
        }

        private int CountFreeNeighbors(HashSet<Vector2Int> rooms, Vector2Int pos)
        {
            int count = 0;
            foreach (var dir in Directions)
            {
                if (!rooms.Contains(pos + dir))
                    count++;
            }
            return count;
        }

        private IEnumerable<Vector2Int> Shuffle(Vector2Int[] dirs)
        {
            var list = new List<Vector2Int>(dirs);
            for (int i = 0; i < list.Count; i++)
            {
                int j = Random.Range(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
            return list;
        }
    }
}