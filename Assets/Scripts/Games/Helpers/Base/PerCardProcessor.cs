using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Games.Helpers
{
	public abstract class PerCardProcessor<PlayerType>
	{
		private IEnumerable<PlayerType> players;
		private int playersCount;

		private int cardsPerPlayer;

		private int cardsToDispatch;
        
		private int nextPlayerIndex;


		public virtual void Configure(IEnumerable<PlayerType> players, int playersCount, int cardsPerPlayer)
		{
			this.players = players;
			this.cardsPerPlayer = cardsPerPlayer;
			this.playersCount = playersCount;

			cardsToDispatch = playersCount * cardsPerPlayer;
			nextPlayerIndex = 0;
		}

		public bool Process()
		{
			if (cardsToDispatch > 0)
			{
                AssignCard (players.ElementAt(nextPlayerIndex));

                return false;
            }

			return true;
		}

		protected virtual void AssignCard (PlayerType player)
		{
			--cardsToDispatch;

            if (++nextPlayerIndex >= playersCount)
                nextPlayerIndex = 0;
        }
	}
}
