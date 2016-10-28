using System.Collections.Generic;

using Players;
using Games.Data;
using Games.Conditions;


namespace Games
{
	public abstract class GameBase<GameDataType> where GameDataType : GameData
    {
		protected GameDataType data;


		protected GameBase(GameDataType data)
		{
			this.data = data;
		}

		protected Player CheckWinner(IEnumerable<Player> players)
        {
			foreach (GCBase condition in data.WinConditions)
            {
				Player winner = condition.Pass(players);
                if (winner != null)
                    return winner;
            }

            return null;
        }

		protected Player CheckLoser(IEnumerable<Player> players)
        {
			foreach (GCBase condition in data.LoseConditions)
            {
				Player loser = condition.Pass(players);
                if (loser != null)
                    return loser;
            }

            return null;
        }

		public abstract void StartGame(int playerCount);
		public abstract bool UpdateGame();
    }
}
