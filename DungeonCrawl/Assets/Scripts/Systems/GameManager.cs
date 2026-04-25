using System.Linq;
using DefaultNamespace.Systems;
using DefaultNamespace.Systems.Data;
using Generation.Dungeon;
using Systems.Dungeon;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private IDungeonManager _dungeonManager;
    [Inject] private IPlayerManager _playerManager;

    private void Start()
    {
        var dungeon = _dungeonManager.GenerateDungeon();

        var startRoom = dungeon.Rooms.Single(r => r.Type == RoomType.Start);

        var player = new PlayerData(initialRoom: startRoom);

        _playerManager.InitializePlayer(player);
    }
}