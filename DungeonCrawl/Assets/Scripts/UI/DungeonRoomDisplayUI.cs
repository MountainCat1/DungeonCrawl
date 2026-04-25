using Systems.Dungeon.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonRoomDisplayUI : MonoBehaviour
{
    [SerializeField] private Image roomImage;
    [SerializeField] private TMP_Text roomNameText;
    
    public void Initialize(DungeonRoom room) 
    {
        roomNameText.text = room.Type.ToString();
        roomImage.color = Color.white;
    }
}
