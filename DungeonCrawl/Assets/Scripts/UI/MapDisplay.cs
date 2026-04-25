using System.Collections.Generic;
using Systems.Dungeon;
using Systems.Dungeon.Data;
using UnityEngine;
using Zenject;

public class MapDisplay : MonoBehaviour
{
    [Inject] private IDungeonManager _dungeonManager;
    
    [SerializeField] private GameObject roomUIPrefab;
    [SerializeField] private GameObject connectionUIPrefab;
    [SerializeField] private Transform roomUIParent;

    [SerializeField] private float cellSize = 50f;
    private void Start()
    {
        _dungeonManager.OnDungeonGenerated += OnDungeonGenerated;
    }

    private void OnDungeonGenerated(Dungeon dungeon)
    {
        UpdateDisplay(dungeon);
    }


    private void UpdateDisplay(Dungeon dungeon)
    {
        foreach (Transform child in roomUIParent)
            Destroy(child.gameObject);

        // draw rooms
        foreach (var room in dungeon.Rooms)
        {
            var go = Instantiate(roomUIPrefab, roomUIParent);
            var rect = go.GetComponent<RectTransform>();

            rect.anchoredPosition = ToUIPos(room.Position);
        }

        // draw connections

        var drawn = new HashSet<(DungeonRoom, DungeonRoom)>();

        foreach (var room in dungeon.Rooms)
        {
            foreach (var neighbour in room.Neighbours)
            {
                var edge = OrderEdge(room, neighbour);

                if (drawn.Contains(edge))
                    continue;

                drawn.Add(edge);

                DrawConnection(room, neighbour);
            }
        }
    }
    
    private void DrawConnection(DungeonRoom a, DungeonRoom b)
    {
        var go = Instantiate(connectionUIPrefab, roomUIParent);
        var rect = go.GetComponent<RectTransform>();

        var posA = ToUIPos(a.Position);
        var posB = ToUIPos(b.Position);

        var mid = (posA + posB) * 0.5f;
        var dir = posB - posA;

        rect.anchoredPosition = mid;

        // length
        rect.sizeDelta = new Vector2(dir.magnitude, rect.sizeDelta.y);

        // rotation
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rect.localRotation = Quaternion.Euler(0, 0, angle);
    }
    
    private Vector2 ToUIPos(Vector2Int gridPos)
    {
        return new Vector2(
            gridPos.x * cellSize,
            gridPos.y * cellSize
        );
    }

    private (DungeonRoom, DungeonRoom) OrderEdge(DungeonRoom a, DungeonRoom b)
    {
        var pa = a.Position;
        var pb = b.Position;

        return pa.x < pb.x || (pa.x == pb.x && pa.y < pb.y)
            ? (a, b)
            : (b, a);
    }
}