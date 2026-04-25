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

        [Header("Setup")]
        [SerializeField] private List<Card> initialCards;
        [SerializeField] private CardUI cardUIPrefab;
        [SerializeField] private Transform cardContainer;

        [Header("Layout")]
        [SerializeField] private float radius = 500f;
        [SerializeField] private float maxAngle = 60f; // total spread (degrees)
        [SerializeField] private float offsetY = -500f; // total spread (degrees)

        private readonly List<CardUI> _cards = new();

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

            _cards.Add(cardUI);
            UpdateLayout();
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

                card.localPosition = new Vector3(x, y, 0) + new Vector3(0, offsetY, 0);

                // Make card "point" toward center (container)
                Vector3 dirToCenter = -card.localPosition;
                float rotZ = Mathf.Atan2(dirToCenter.y, dirToCenter.x) * Mathf.Rad2Deg + 90f;

                card.localRotation = Quaternion.Euler(0, 0, rotZ);

                card.SetSiblingIndex(i);
            }
        }
    }
}