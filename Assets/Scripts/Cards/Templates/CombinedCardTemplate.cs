using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Cards
{
	public class CombinedCardTemplate : MonoBehaviour, ICardTemplate
    {
        #region internal data types

        [Serializable]
        private class ColorData
        {
            public Color red;
            public Color black;
        }

        [Serializable]
        private class IconData
        {
            public Sprite clubs;
            public Sprite diamonds;
            public Sprite hearts;
            public Sprite spades;
        }

        [Serializable]
        private class ImageData
        {
            public Sprite jack;
            public Sprite queen;
            public Sprite king;
        }

        [Serializable]
        private class CardData
        {
            public Sprite frame;
            public Sprite back;
        }

        [Serializable]
        private class CommonData
        {
            public GameObject iconsLayout;
            public GameObject imagesLayout;

            public int iconSortingOrder = 1;
            public int imageSortingOrder = 2;
            public int backSortingOrder = -1;
            public int frameSortingOrder = 0;

            public IconLayoutData layoutData;
        }

        [Serializable]
        private class IconLayoutData
        {
            public int      columnsCount;
            public Vector2  layoutSize;
        }

        #endregion

        #region Inspector Data

        [SerializeField]
        private ColorData           colorData;

        [SerializeField]
        private IconData            iconData;

        [SerializeField]
        private ImageData           imageData;

        [SerializeField]
        private CardData            cardData;

        [SerializeField]
        private CommonData          commonData;

        #endregion

        private Renderer[]          allRenderers;

        private TextMesh[]          titles;
        private SpriteRenderer[]    icons;
        private SpriteRenderer[]    images;

        private SpriteRenderer      back;
        private SpriteRenderer[]    frames;

        #region Unity Events

        private void Awake()
        {
            allRenderers = GetComponentsInChildren<Renderer>(true);
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>(true);

            icons =     sprites.Where(s => s.sortingOrder == commonData.iconSortingOrder).ToArray();
            images =    sprites.Where(s => s.sortingOrder == commonData.imageSortingOrder).ToArray();
            back =      sprites.Single(s => s.sortingOrder == commonData.backSortingOrder);
            frames =    sprites.Where(s => s.sortingOrder == commonData.frameSortingOrder).ToArray();

            titles = GetComponentsInChildren<TextMesh>(true);
        }

        private void OnDestroy()
        {
            titles =    null;
            icons =     null;
            images =    null;
        }

        #endregion

        public void Configure(Card.Suit suit, Card.Rank rank, Card.State state)
        {
            if (state == Card.State.Closed)
                ForceFlip();

            ChangeColor(suit);
            SetIconsSprite(GetIconBySuit(suit));
            SetTitlesText(GetTitleTextByRank(rank));

            if (rank < Card.Rank.Jack && rank > Card.Rank.King)
                ConfigureIconLayout(rank);
            else
                ConfigureImageLayout(rank);

            back.sprite = cardData.back;

            foreach (var frame in frames)
                frame.sprite = cardData.frame;
        }

        public void Flip()
        {
            foreach (var r in allRenderers)
                r.sortingOrder = - r.sortingOrder;
        }

        public void ForceFlip()
        {
            this.transform.Rotate(new Vector3(0, 180, 0));
            Flip();
        }

		public GameObject GetObject()
		{
			return this.gameObject;
		}

        public void SetSortingOrderOffset(int offset)
        {
            foreach (var r in allRenderers)
                r.sortingOrder += offset;
        }

        private void ChangeColor(Card.Suit suit)
        {
            Color color;

            if (suit == Card.Suit.Diamonds || suit == Card.Suit.Hearts)
                color = colorData.red;
            else
                color = colorData.black;

            foreach (var title in titles)
                title.color = color;

            foreach (var icon in icons)
                icon.color = color;

            foreach (var image in images)
                image.color = color;
        }

        #region Icon Layout Setup

        private void ConfigureIconLayout(Card.Rank rank)
        {
            commonData.iconsLayout.SetActive(false);
            commonData.imagesLayout.SetActive(true);

            if (rank < Card.Rank.Ace)
                GenerateIconLayout(ReactivateIconsCount((int)rank));
            else
                GenerateIconLayout(ReactivateIconsCount(1));
        }

        private void GenerateIconLayout(List<Transform> children)
        {
            Vector2 layoutSize = commonData.layoutData.layoutSize;
            Vector2 nextIconPosition = - layoutSize / 2;

            ///TODO: layout logic
        }

        private List<Transform> ReactivateIconsCount(int count)
        {
            List<Transform> instances = new List<Transform>();

            int index = 0;

            foreach (Transform child in commonData.iconsLayout.transform)
            {
                if (index < count)
                {
                    child.gameObject.SetActive(true);
                    instances.Add(child);
                }
                else
                    child.gameObject.SetActive(false);

                index++;
            }

            return instances;
        }

        #endregion

        #region Images Layout Setup

        private void ConfigureImageLayout(Card.Rank rank)
        {
            commonData.iconsLayout.SetActive(false);
            commonData.imagesLayout.SetActive(true);

            foreach (SpriteRenderer image in images)
                image.sprite = GetImageByRank(rank);
        }

        private Sprite GetImageByRank(Card.Rank rank)
        {
            switch (rank)
            {
                case Card.Rank.Jack:
                    return imageData.jack;

                case Card.Rank.Queen:
                    return imageData.queen;

                case Card.Rank.King:
                    return imageData.king;
            }

            return null;
        }

        #endregion

        #region Icons Setup

        private Sprite GetIconBySuit(Card.Suit suit)
        {
            switch (suit)
            {
                case Card.Suit.Clubs:
                    return iconData.clubs;

                case Card.Suit.Diamonds:
                    return iconData.diamonds;

                case Card.Suit.Hearts:
                    return iconData.hearts;

                case Card.Suit.Spades:
                    return iconData.spades;
            }

            return null;
        }

        private void SetIconsSprite(Sprite sprite)
        {
            foreach (SpriteRenderer icon in icons)
                icon.sprite = sprite;
        }

        #endregion

        #region Titles Setup

        private string GetTitleTextByRank(Card.Rank rank)
        {
            if (rank < Card.Rank.Jack)
                return ((int)rank).ToString();

            return Enum.GetName(typeof(Card.Rank), rank).Substring(0, 1).ToUpper();
        }

        private void SetTitlesText(string text)
        {
            foreach (TextMesh title in titles)
                title.text = text;
        }

        #endregion
    }
}
