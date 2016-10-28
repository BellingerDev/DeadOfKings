using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

using Utils;
using Cards;


namespace Controllers
{
    public class DeckController : SingletonMonoBehaviour<DeckController>
    {
		[SerializeField]
		private Transform deckRoot;


		private List<Card> cardInstances;

		public bool IsGenerated { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            cardInstances = new List<Card>();
			IsGenerated = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

			foreach (var card in cardInstances)
				Pool.Instance.Retrieve (card.gameObject);

            cardInstances.Clear();
            cardInstances = null;
        }

		public void Generate(ICardTemplate template, Card.State state)
        {
            int suitCount = Enum.GetValues(typeof(Card.Suit)).Length;
            int rankCount = Enum.GetValues(typeof(Card.Rank)).Length;

            for (int i = 0; i < suitCount; i++)
            {
                for (int j = 0; j < rankCount; j++)
                {
					Card card = GetCardFromPool();
					ICardTemplate templateInstance = GetTemplateFromPool (template);

					card.Configure((Card.Suit)i, (Card.Rank)j, state, templateInstance);

                    cardInstances.Add(card);
                }
            }

			IsGenerated = true;
        }

        public void Shuffle()
        {
            cardInstances.Sort();
        }

        public void RetrieveCard(Card card)
        {
            card.GetTemplate().GetObject().SetActive(false);
            card.GetTemplate().GetObject().transform.SetParent(deckRoot);
            cardInstances.Add(card);
        }

        public Card GetCard()
        {
			if (cardInstances.Count > 0)
			{
				Card card = cardInstances [0];
				cardInstances.Remove (card);

				card.GetTemplate ().GetObject ().SetActive (true);

				return card;
			}

			return null;
        }

		public Card GenerateRandomCard(Card.State state, ICardTemplate template)
        {
            int suitCount = Enum.GetValues(typeof(Card.Suit)).Length;
            int rankCount = Enum.GetValues(typeof(Card.Rank)).Length;

            Card card = GetCardFromPool();
			ICardTemplate templateInstance = GetTemplateFromPool (template);

			card.Configure((Card.Suit)Random.Range(0, suitCount), (Card.Rank)Random.Range(0, rankCount), state, templateInstance);
			card.GetTemplate ().GetObject ().SetActive (true);

            return card;
        }

        private Card GetCardFromPool()
        {
            GameObject cardGO = Pool.Instance.Get(CommonPrototypesController.Instance.Card);

			cardGO.transform.SetParent(deckRoot);
            cardGO.transform.localPosition = Vector3.zero;
            cardGO.transform.localRotation = Quaternion.identity;
            cardGO.SetActive(true);

            Card card = cardGO.GetComponent<Card>();

            return card;
        }

		private ICardTemplate GetTemplateFromPool(ICardTemplate template)
		{
			GameObject templateInstance = Pool.Instance.Get (template.GetObject ());

			templateInstance.transform.SetParent (deckRoot);
			templateInstance.transform.localPosition = Vector3.zero;
			templateInstance.transform.localRotation = Quaternion.identity;

			return templateInstance.GetComponent<ICardTemplate> ();
		}
    }
}
