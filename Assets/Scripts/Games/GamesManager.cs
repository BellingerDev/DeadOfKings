using System.Collections.Generic;
using UnityEngine;

using Players;
using Games.Data;
using Utils;
using UI;
using UI.Screens;


namespace Games
{
	public class GamesManager : SingletonMonoBehaviour<GamesManager>
	{
		#region Events

		public class GameEndEvent : EventManager.IEvent
		{
			public Player targetPlayer;
			public Result result;

			public Dictionary<string, object> extra = new Dictionary<string, object>();
		}

		public class GameStartedEvent : EventManager.IEvent
		{
			public IEnumerable<Player> players;
		}

		public class GameStartEvent : EventManager.IEvent
		{
			public int playersCount;
		}

		#endregion

		public enum Result
		{
			Win, Lose, DeadHeat
		}

		[SerializeField]
		private DeadOfKingsData deadOfKingsData;

		private DeadOfKingsGame deadOfKingsGame;

		public bool IsGame { get; private set; }


		protected override void Awake()
		{
			base.Awake ();

			UIController.Instance.Init ();
			UIController.Instance.Show<UIMainMenuScreen> ();

			deadOfKingsGame = new DeadOfKingsGame (deadOfKingsData);

			EventManager.Attach<GameStartEvent> (OnGameStart);
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			EventManager.Detach<GameStartEvent> (OnGameStart);
		}

		private void Update()
		{
			if (IsGame)
				if (!deadOfKingsGame.UpdateGame ())
					IsGame = false;
		}

		private void OnGameStart(GameStartEvent e)
		{
            IsGame = true;
			deadOfKingsGame.StartGame (e.playersCount);
		}

		public int GetDeadOfKingsMinPlayers()
		{
			return deadOfKingsData.MinPlayers;
		}

		public int GetDeadOfKingsMaxPlayers()
		{
			return deadOfKingsData.MaxPlayers;
		}

        public PlayersDatabase GetDeadOfKingsDatabase()
        {
            return deadOfKingsData.PlayersDB;
        }
	}
}

