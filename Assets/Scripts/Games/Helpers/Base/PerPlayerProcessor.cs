using System.Collections.Generic;
using System.Linq;


namespace Games.Helpers
{
	public abstract class PerPlayerProcessor<PlayerType>
	{
		private IEnumerable<PlayerType> players;
		private int playerCount;
        
		private int nextPlayerIndex;


		public virtual void Configure(IEnumerable<PlayerType> players, int playerCount)
		{			
			this.players = players;
			this.playerCount = playerCount;
		}

		public bool Process(bool isOpen)
		{
			if (nextPlayerIndex < playerCount)
			{
				if (isOpen)
					Open (players.ElementAt(nextPlayerIndex));
				else
					Close (players.ElementAt(nextPlayerIndex));

				return false;
			}

			return true;
		}

		protected virtual void Open(PlayerType player)
		{
			nextPlayerIndex++;
		}

		protected virtual void Close(PlayerType player)
		{
			nextPlayerIndex++;
		}
	}
}

