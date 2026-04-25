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
        var drawn = new HashSet<(Vector2Int, Vector2Int)>();

        foreach (var kvp in dungeon.Connections)
        {
            var from = kvp.Key;

            foreach (var to in kvp.Value)
            {
                // avoid drawing duplicates (A-B and B-A)
                var edge = OrderEdge(from, to);
                if (drawn.Contains(edge))
                    continue;

                drawn.Add(edge);

                DrawConnection(from, to);
            }
        }
    }
    
    private void DrawConnection(Vector2Int a, Vector2Int b)
    {
        var go = Instantiate(connectionUIPrefab, roomUIParent);
        var rect = go.GetComponent<RectTransform>();

        var posA = ToUIPos(a);
        var posB = ToUIPos(b);

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

    private (Vector2Int, Vector2Int) OrderEdge(Vector2Int a, Vector2Int b)
    {
        return a.x < b.x || (a.x == b.x && a.y < b.y)
            ? (a, b)
            : (b, a);
    }
}