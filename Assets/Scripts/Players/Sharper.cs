using UnityEngine;
using System.Collections.Generic;

using CardFans;
using Cards;


namespace Players
{
    public class Sharper : Player
    {
        [SerializeField]
		private CardsChildrenFan sleeveFan;

		private List<Card> sleeveCards;


		public override void Configure (string id)
		{
			base.Configure (id);

			if (sleeveCards == null)
				sleeveCards = new List<Card> ();
		}

        public void AddCardToSleeve(Card card)
        {
            sleeveCards.Add(card);
            sleeveFan.Add(card.GetTemplate().GetObject());
        }

        public void RemoveCardFromSleeve(Card card)
        {
            sleeveCards.Remove(card);
            sleeveFan.Remove(card.GetTemplate().GetObject());
        }

		public IEnumerable<Card> GetSleeveCards()
		{
			return sleeveCards;
		}

        public override void UpdateFanLayout()
        {
            base.UpdateFanLayout();
            sleeveFan.UpdateLayout();
        }

        public override bool IsFanLayoutUpdating()
        {
            return sleeveFan.IsLayoutUpdating;
        }
    }
}
