using UnityEngine;

namespace DefaultNamespace.Cards
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Custom/Card")]
    public class Card : ScriptableObject
    {
        [field: SerializeField] public string NameTag { get; set; }
        [field: SerializeField] public string DescriptionTag { get; set; }
        [field: SerializeField] public Sprite Sprite { get; set; }
    }
}