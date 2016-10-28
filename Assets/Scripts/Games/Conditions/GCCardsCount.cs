using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Players;
using Cards;


namespace Games.Conditions
{
    [CreateAssetMenu(fileName = "GCCardsCount", menuName = "Data/GameConditions/GCCardsCount")]
	public class GCCardsCount : GCBase
    {
        [SerializeField]
        private Card.Rank[] ranks;

        [Tooltip(">, <, >=, <=, ==, !=")]
        [SerializeField]
        private string compareCondition;


		public override Player Pass(IEnumerable<Player> players)
        {
            Player passedPlayer = null;
			int count = CompareByCondition (0, int.MaxValue, compareCondition) ? 0 : int.MaxValue;

            foreach (var player in players)
            {
                int newCount = GetCardsCountByRanks(player.GetHandCards());
                
                if (CompareByCondition(count, newCount, compareCondition))
                {
                    passedPlayer = player;
                    count = newCount;
                }
            }

            return passedPlayer;
        }

		private int GetCardsCountByRanks(IEnumerable<Card> cards)
        {
            Card[] comparedCards = cards.Where(c => ranks.Contains(c.GetRank())).ToArray();
            return comparedCards.Length;
        }
    }
}
