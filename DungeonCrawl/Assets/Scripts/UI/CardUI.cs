using System;
using DefaultNamespace.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Utilities;

namespace DefaultNamespace.UI
{
    public class CardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<CardUI> OnHoverStart;
        public event Action<CardUI> OnHoverEnd;

        public Card Card { get; private set; }
        
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject normalView;

        public virtual void Initialize(Card card)
        {
            nameText.text = LocalizationHelper.L(card.NameTag);
            descriptionText.text = LocalizationHelper.L(card.DescriptionTag);
            iconImage.sprite = card.Sprite;
            Card = card;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetBaseVisible(false);
            OnHoverStart?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetBaseVisible(true);
            OnHoverEnd?.Invoke(this);
        }
        
        private void SetBaseVisible(bool visible)
        {
            normalView.gameObject.GetComponent<CanvasGroup>().alpha = visible ? 1 : 0;
        }
    }
}