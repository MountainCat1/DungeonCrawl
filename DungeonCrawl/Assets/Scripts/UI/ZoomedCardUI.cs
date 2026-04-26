using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class ZoomedCardUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Image iconImage;
        [SerializeField] private ParentConstraint parentConstraint;

        public void Initialize(CardUI cardUI)
        {
            nameText.text = cardUI.Card.NameTag;
            descriptionText.text = cardUI.Card.DescriptionTag;
            iconImage.sprite = cardUI.Card.Sprite;

            parentConstraint.SetSource(0, new ConstraintSource
            {
                sourceTransform = cardUI.transform,
                weight = 1
            });
        }
    }
}