using System.Collections.Generic;
using DefaultNamespace.Cards;
using DefaultNamespace.Systems;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.UI
{
    public class CardHandUI : MonoBehaviour
    {
        [Inject] private ISpawnManager _spawnManager;
        [Inject] private ICardPlaySystem _cardPlaySystem;

        [Header("Setup")]
        [SerializeField] private List<Card> initialCards;
        [SerializeField] private CardUI cardUIPrefab;
        [SerializeField] private ZoomedCardUI zoomedCardInstance;
        [SerializeField] private Transform cardContainer;

        [Header("Layout")]
        [SerializeField] private float radius = 500f;
        [SerializeField] private float maxAngle = 60f;
        [SerializeField] private float offsetY = -500f;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotSpeed = 10f;

        [Header("Play")]
        [SerializeField] private float playThresholdY = 200f;

        private readonly List<CardUI> _cards = new();
        private readonly HashSet<CardUI> _draggingCards = new();

        private CardUI _zoomedCard;

        private void Start()
        {
            foreach (var card in initialCards)
                AddCard(card);
        }

        private void Update()
        {
            UpdateLayout();
        }

        public void AddCard(Card card)
        {
            var cardUI = _spawnManager.Spawn(cardUIPrefab, cardContainer);
            cardUI.Initialize(card);

            cardUI.OnHoverStart += OnCardHoverStart;
            cardUI.OnHoverEnd += OnCardHoverEnd;

            cardUI.OnBeginDragEvent += c => _draggingCards.Add(c);
            cardUI.OnEndDragEvent += c => _draggingCards.Remove(c);
            cardUI.OnDragRelease += OnCardReleased;

            _cards.Add(cardUI);
            UpdateLayout();
        }

        private void OnCardHoverStart(CardUI card)
        {
            if (_draggingCards.Contains(card)) return;

            _zoomedCard = card;
            zoomedCardInstance.Initialize(card);
            zoomedCardInstance.gameObject.SetActive(true);

            SetCardUIVisible(card, false);
        }

        private void OnCardHoverEnd(CardUI card)
        {
            if (_zoomedCard != card)
                return;

            _zoomedCard = null;
            zoomedCardInstance.gameObject.SetActive(false);

            SetCardUIVisible(card, true);
        }

        private void OnCardReleased(CardUI card)
        {
            if (card.transform.position.y > playThresholdY)
            {
                PlayCard(card);
            }
        }

        private void PlayCard(CardUI cardUI)
        {
            _cardPlaySystem.Play(cardUI.Card);

            _cards.Remove(cardUI);
            Destroy(cardUI.gameObject);

            zoomedCardInstance.gameObject.SetActive(false);

            UpdateLayout();
        }

        private void SetCardUIVisible(CardUI card, bool visible)
        {
            var cg = card.GetComponent<CanvasGroup>();
            if (cg != null)
                cg.alpha = visible ? 1 : 0;
        }

        private void UpdateLayout()
        {
            int count = _cards.Count;
            if (count == 0) return;

            float step = count > 1 ? maxAngle / (count - 1) : 0f;
            float startAngle = -maxAngle / 2f;

            for (int i = 0; i < count; i++)
            {
                var cardUI = _cards[i];

                if (_draggingCards.Contains(cardUI))
                    continue;

                var card = cardUI.transform;

                float angle = startAngle + step * i;
                float rad = angle * Mathf.Deg2Rad;

                float x = Mathf.Sin(rad) * radius;
                float y = Mathf.Cos(rad) * radius;

                Vector3 targetPos = new Vector3(x, y, 0) + new Vector3(0, offsetY, 0);

                Vector3 dirToCenter = -targetPos;
                float rotZ = Mathf.Atan2(dirToCenter.y, dirToCenter.x) * Mathf.Rad2Deg + 90f;

                Quaternion targetRot = Quaternion.Euler(0, 0, rotZ);

                card.localPosition = Vector3.Lerp(card.localPosition, targetPos, Time.deltaTime * moveSpeed);
                card.localRotation = Quaternion.Slerp(card.localRotation, targetRot, Time.deltaTime * rotSpeed);

                card.SetSiblingIndex(i);
            }
        }
    }
}