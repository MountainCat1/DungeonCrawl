using DefaultNamespace.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace DefaultNamespace.UI
{
    public class CardUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;
        
        public void Initialize(Card card)
        {
            nameText.text = LocalizationHelper.L(card.NameTag);
            descriptionText.text = LocalizationHelper.L(card.DescriptionTag);
            iconImage.sprite = card.Sprite;
        }
    }
}