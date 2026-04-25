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
            ctx.Connections.Clear();

            var start = new RoomGenerationData
            {
                Position = Vector2Int.zero,
                Type = RoomType.Start
            };

            ctx.Rooms.Add(start);

            var frontier = new List<RoomGenerationData> { start };

            while (ctx.Rooms.Count < targetRooms && frontier.Count > 0)
            {
                var current = frontier[Random.Range(0, frontier.Count)];

                foreach (var dir in Shuffle(Directions))
                {
                    var nextPos = current.Position + dir;

                    if (GetRoomAt(ctx, nextPos) != null)
                        continue;

                    if (CountConnections(ctx, current) >= maxNeighbors)
                        continue;

                    if (CountNeighbors(ctx, nextPos) > 1)
                        continue;

                    var next = new RoomGenerationData
                    {
                        Position = nextPos,
                        Type = RoomType.Normal
                    };

                    ctx.Rooms.Add(next);
                    frontier.Add(next);

                    Connect(ctx, current, next);

                    if (ctx.Rooms.Count >= targetRooms)
                        break;
                }

                if (CountFreeNeighbors(ctx, current.Position) == 0)
                    frontier.Remove(current);
            }

            PlaceSpecialRooms(ctx);
            AddExtraConnections(ctx);
        }

        private RoomGenerationData GetRoomAt(DungeonGenerationContext ctx, Vector2Int pos)
        {
            foreach (var room in ctx.Rooms)
            {
                if (room.Position == pos)
                    return room;
            }
            return null;
        }

        private int CountNeighbors(DungeonGenerationContext ctx, Vector2Int pos)
        {
            int count = 0;

            foreach (var dir in Directions)
            {
                if (GetRoomAt(ctx, pos + dir) != null)
                    count++;
            }

            return count;
        }

        private int CountFreeNeighbors(DungeonGenerationContext ctx, Vector2Int pos)
        {
            int count = 0;

            foreach (var dir in Directions)
            {
                if (GetRoomAt(ctx, pos + dir) == null)
                    count++;
            }

            return count;
        }

        private int CountConnections(DungeonGenerationContext ctx, RoomGenerationData room)
        {
            return ctx.Connections.TryGetValue(room, out var set) ? set.Count : 0;
        }

        private void Connect(DungeonGenerationContext ctx, RoomGenerationData a, RoomGenerationData b)
        {
            if (!ctx.Connections.TryGetValue(a, out var listA))
            {
                listA = new HashSet<RoomGenerationData>();
                ctx.Connections[a] = listA;
            }

            if (!ctx.Connections.TryGetValue(b, out var listB))
            {
                listB = new HashSet<RoomGenerationData>();
                ctx.Connections[b] = listB;
            }

            listA.Add(b);
            listB.Add(a);
        }

        private void AddExtraConnections(DungeonGenerationContext ctx)
        {
            foreach (var room in ctx.Rooms)
            {
                foreach (var dir in Directions)
                {
                    var neighbor = GetRoomAt(ctx, room.Position + dir);

                    if (neighbor == null)
                        continue;

                    if (ctx.Connections.TryGetValue(room, out var conns) && conns.Contains(neighbor))
                        continue;

                    if (CountConnections(ctx, room) >= maxNeighbors)
                        continue;

                    if (CountConnections(ctx, neighbor) >= maxNeighbors)
                        continue;

                    if (Random.value < extraConnectionChance)
                    {
                        Connect(ctx, room, neighbor);
                    }
                }
            }
        }

        private void PlaceSpecialRooms(DungeonGenerationContext ctx)
        {
            var deadEnds = new List<RoomGenerationData>();

            foreach (var room in ctx.Rooms)
            {
                if (CountConnections(ctx, room) == 1 && room.Type == RoomType.Normal)
                    deadEnds.Add(room);
            }

            if (deadEnds.Count == 0)
                return;

            var boss = deadEnds[Random.Range(0, deadEnds.Count)];
            boss.Type = RoomType.Boss;
            deadEnds.Remove(boss);

            if (deadEnds.Count > 0)
            {
                var treasure = deadEnds[Random.Range(0, deadEnds.Count)];
                treasure.Type = RoomType.Treasure;
                deadEnds.Remove(treasure);
            }

            if (deadEnds.Count > 0)
            {
                var shop = deadEnds[Random.Range(0, deadEnds.Count)];
                shop.Type = RoomType.Shop;
            }
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