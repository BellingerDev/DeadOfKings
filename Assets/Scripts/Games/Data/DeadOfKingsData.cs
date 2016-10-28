using UnityEngine;


namespace Games.Data
{
	[CreateAssetMenu(fileName = "DeadOfKingsData", menuName = "Data/Games/DeadOfKingsData")]
	public class DeadOfKingsData : GameData
	{
		[SerializeField]
		private int scoreReward;

		[SerializeField]
		private int handCardsCount;

		[SerializeField]
		private int sleeveCardsCount;

		[SerializeField]
		private string userPlayerId;

		[SerializeField]
		private int minPlayers;

		[SerializeField]
		private int maxPlayers;


		public int ScoreReward { get { return scoreReward; } }
		public int HandCardsCount { get { return handCardsCount; } }
		public int SleeveCardsCount { get { return sleeveCardsCount; } }
		public string UserPlayerId { get { return userPlayerId; } }
		public int MinPlayers { get { return minPlayers; } }
		public int MaxPlayers { get { return maxPlayers; } }
	}
}

