using UnityEngine;


namespace Cards
{
	public interface ICardTemplate
	{
		void Configure (Card.Suit suit, Card.Rank rank, Card.State state);
		void ForceFlip();
		GameObject GetObject();
	}
}

