using Systems.Dungeon;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private IDungeonManager _dungeonManager;

    private void Start()
    {
        _dungeonManager.GenerateDungeon();
    }
}
