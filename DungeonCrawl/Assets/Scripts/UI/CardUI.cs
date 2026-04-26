using System;
using DefaultNamespace.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;

namespace DefaultNamespace.UI
{
    public class CardUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        public event Action<CardUI> OnHoverStart;
        public event Action<CardUI> OnHoverEnd;
        public event Action<CardUI> OnBeginDragEvent;
        public event Action<CardUI> OnEndDragEvent;
        public event Action<CardUI> OnDragRelease;

        public Card Card { get; private set; }

        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject normalView;

        private RectTransform _rect;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        private Vector3 _startPos;

        private void Awake()
        {
            _rect = transform as RectTransform;
            _canvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

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

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startPos = _rect.position;

            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.85f;

            SetBaseVisible(true);
            OnHoverEnd?.Invoke(this);
            OnBeginDragEvent?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rect.position += (Vector3)eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;

            OnEndDragEvent?.Invoke(this);
            OnDragRelease?.Invoke(this);
        }

        private void SetBaseVisible(bool visible)
        {
            var cg = normalView.GetComponent<CanvasGroup>();
            if (cg != null)
                cg.alpha = visible ? 1 : 0;
        }
    }
}