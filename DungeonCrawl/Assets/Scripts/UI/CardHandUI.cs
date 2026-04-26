using System;
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

        [Header("Setup")] [SerializeField] private List<Card> initialCards;
        [SerializeField] private CardUI cardUIPrefab;
        [SerializeField] private ZoomedCardUI zoomedCardInstance;
        [SerializeField] private Transform cardContainer;

        [Header("Layout")] [SerializeField] private float radius = 500f;
        [SerializeField] private float maxAngle = 60f; // total spread (degrees)
        [SerializeField] private float offsetY = -500f; // total spread (degrees)
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotSpeed = 10f;

        private readonly List<CardUI> _cards = new();
        private CardUI _zoomedCard;

        private void Start()
        {
            foreach (var card in initialCards)
            {
                AddCard(card);
            }
        }

        private void Update()
        {
            UpdateLayout();
        }

        public void AddCard(Card card)
        {
            var cardUI = _spawnManager.Spawn(cardUIPrefab, cardContainer);
            cardUI.Initialize(card);
            cardUI.OnHoverEnd += OnCardUIHoverEnd;
            cardUI.OnHoverStart += OnCardUIHoverStart;

            _cards.Add(cardUI);
            UpdateLayout();
        }

        private void OnCardUIHoverStart(CardUI card)
        {
            _zoomedCard = card;
            zoomedCardInstance.Initialize(card);
            zoomedCardInstance.gameObject.SetActive(true);
            
            SetCardUIVisible(card, false);
        }

        private void SetCardUIVisible(CardUI card, bool b)
        {
            var canvasGroup = card.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
                canvasGroup.alpha = b ? 1 : 0;
        }

        private void OnCardUIHoverEnd(CardUI card)
        {
            if (_zoomedCard != card)
                return;

            _zoomedCard = null;
            zoomedCardInstance.gameObject.SetActive(false);
            SetCardUIVisible(card, true);
        }

        private void UpdateLayout()
        {
            int count = _cards.Count;
            if (count == 0) return;

            float step = count > 1 ? maxAngle / (count - 1) : 0f;
            float startAngle = -maxAngle / 2f;

            for (int i = 0; i < count; i++)
            {
                var card = _cards[i].transform;

                float angle = startAngle + step * i;
                float rad = angle * Mathf.Deg2Rad;

                // Position on arc (circle around container)
                float x = Mathf.Sin(rad) * radius;
                float y = Mathf.Cos(rad) * radius;


                // Make card "point" toward center (container)
                Vector3 dirToCenter = -card.localPosition;
                float rotZ = Mathf.Atan2(dirToCenter.y, dirToCenter.x) * Mathf.Rad2Deg + 90f;


                Vector3 targetPos = new Vector3(x, y, 0) + new Vector3(0, offsetY, 0);
                Quaternion targetRot = Quaternion.Euler(0, 0, rotZ);

                // drift toward target
                card.localPosition = Vector3.Lerp(card.localPosition, targetPos, Time.deltaTime * moveSpeed);
                card.localRotation = Quaternion.Slerp(card.localRotation, targetRot, Time.deltaTime * rotSpeed);


                card.SetSiblingIndex(i);
            }
        }
    }
}