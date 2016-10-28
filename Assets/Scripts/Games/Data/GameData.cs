using UnityEngine;
using System.Collections.Generic;

using Games.Conditions;
using Cards;


namespace Games.Data
{
	public class GameData : ScriptableObject
	{
		[SerializeField]
		private PlayersDatabase playersDB;

		[SerializeField]
		private GameObject cardTemplate;

		[SerializeField]
		private GCBase[] winConditions;

		[SerializeField]
		private GCBase[] loseConditions;

		private ICardTemplate iTemplate;


		public ICardTemplate Template 
		{ 
			get 
			{ 
				if (iTemplate == null)
					iTemplate = cardTemplate.GetComponent<ICardTemplate> ();

				return iTemplate;
			}
		}

		public IEnumerable<GCBase> WinConditions { get { return winConditions; } }
		public IEnumerable<GCBase> LoseConditions { get { return loseConditions; } }
		public PlayersDatabase PlayersDB { get { return playersDB; } }
	}
}

