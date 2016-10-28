using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Players;
using Cards;


namespace Games.Conditions
{
    [CreateAssetMenu(fileName = "GCCardDublicates", menuName = "Data/GameConditions/GCCardDublicates")]
    public class GCCardDublicates : GCBase
    {
        [SerializeField]
        private int countToPass;

        [Tooltip(">, <, >=, <=, ==, !=")]
        [SerializeField]
        private string compareCondition;


		public override Player Pass(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                Card[] passed = player.GetHandCards()
                    .GroupBy(c => c.GetRank())
                    .Where(g => CompareByCondition(compareCondition.Count(), countToPass, compareCondition))
                    .SelectMany(g => g)
                    .ToArray();

                passed = passed
                    .GroupBy(c => c.GetSuit())
                    .Where(g => CompareByCondition(compareCondition.Count(), countToPass, compareCondition))
                    .SelectMany(g => g)
                    .ToArray();

                if (passed.Length > 0)
                    return player;
            }

            return null;
        }
    }
}
