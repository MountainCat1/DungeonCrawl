using DefaultNamespace.Systems;
using Systems.Dungeon.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DungeonRoomDisplayUI : MonoBehaviour
{
    [Inject] private IPlayerManager _playerManager;
    
    public DungeonRoom DungeonRoom { get; private set; }
    
    [SerializeField] private Image roomImage;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private Button button;
    
    public void Initialize(DungeonRoom room) 
    {
        DungeonRoom = room;
        
        roomNameText.text = room.Type.ToString();
        roomImage.color = Color.white;

        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        _playerManager.Player.MoveToRoom(DungeonRoom);
    }
}
