using UnityEngine;
using System.Collections.Generic;

using CardFans;
using Cards;
using Controllers;


namespace Players
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
		private CardsChildrenFan handFan;

		private string id;
		private int score;

		private List<Card> handCards;


		public virtual void Configure(string id)
		{
			this.id = id;
            this.score = 0;

			if (handCards == null)
                handCards = new List<Card> ();
		}

        public void AddCardToHand(Card card)
        {
            handCards.Add(card);
            handFan.Add(card.GetTemplate().GetObject());
        }

        public void RemoveCardFromHand(Card card)
        {
            handFan.Remove(card.GetTemplate().GetObject());
            handCards.Remove(card);
        }

		public IEnumerable<Card> GetHandCards()
        {
			return handCards;
        }

        public void AddScore(int value)
        {
            score += value;
        }

        public string GetId()
        {
            return id;
        }

        public int GetScore()
        {
            return score;
        }

        public virtual void UpdateFanLayout()
        {
            handFan.UpdateLayout();
        }

        public virtual bool IsFanLayoutUpdating()
        {
            return handFan.IsLayoutUpdating;
        }
    }
}
