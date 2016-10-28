using System;
using System.Collections.Generic;
using UnityEngine;

using Games.Data;
using Games.Helpers;
using Controllers;
using Cards;
using Players;


namespace Games
{
	public class DeadOfKingsGame : GameBase<DeadOfKingsData>
	{
		public const string EXTRA_PHASE = "Phase";
		public const string EXTRA_SCORE_REWARD = "ScoreReward";

		#region Types

		public enum Phase
		{
			Dispatch, Thinking, Opening, ScoreRewards
		}

		#endregion

		#region Events

        public class StartThinkingEvent : EventManager.IEvent { }
		public class EndThinkingEvent : EventManager.IEvent { }

		#endregion

		private Phase phase;

        #region Helpers

        private HandCardsDispatcher handDispatcher;
		private SleeveCardsDispatcher sleeveDispatcher;

		private HandCardsOpener allHandCardsOpener;

        private HandCardsOpener userHandCardsOpener;
		private SleeveCardsOpener userSleeveCardsOpener;

        private HandCardsRetriever allHandCardsRetriever;
        private SleeveCardsRetriever userSleeveCardsRetriever;

        private HandCardsLayoutUpdater allHandCardsLayoutUpdater;
        private SleeveCardsLayoutUpdater userSleeveCardsLayoutUpdater;

        private HandCardsHighlighter winnerCardsHighlighter;

        #endregion

        private bool isThinking;
        private bool isWaitingForEndThinking;
        private bool isWinnerScoreRewarded;

		private int playerCount;


		public DeadOfKingsGame(DeadOfKingsData data) 
			: base (data) { }

		#region implemented abstract members of GameBase

		public override void StartGame (int playerCount)
		{
			this.playerCount = playerCount;

			Configure ();
		}

		private void Configure()
		{
			if (!DeckController.Instance.IsGenerated)
				DeckController.Instance.Generate (data.Template, Card.State.Closed);

			DeckController.Instance.Shuffle ();

            if (BoardController.Instance.GetPlayerCount() != playerCount + 1)
                BoardController.Instance.ClearBoard();

            CreatePlayers(playerCount);

            isThinking = false;
            isWaitingForEndThinking = false;
            isWinnerScoreRewarded = false;

			phase = Phase.Dispatch;

            Debug.Log("Game Configurated");
		}

		public override bool UpdateGame ()
		{
			Player winner = null, loser = null;

			switch (phase)
			{
				case Phase.Dispatch:
					if (Dispatch ())
					{
						loser = CheckLoser (BoardController.Instance.GetPlayers ());
						phase = Phase.Thinking;
					}
					break;

				case Phase.Thinking:
					if (Thinking ())
						phase = Phase.Opening;
					break;

				case Phase.Opening:
					if (Opening ())
                        phase = Phase.ScoreRewards;
					break;

				case Phase.ScoreRewards:
                    if (!isWinnerScoreRewarded)
                    {
                        loser = CheckLoser(BoardController.Instance.GetPlayers());

                        if (loser == null)
                            winner = CheckWinner(BoardController.Instance.GetPlayers());
                    }

                    if (ScoreReward (winner))
						Configure ();
                    break;
			}

			return CheckEndOfGame (winner, loser);
		}

		#endregion

		#region Phase Methods

		private bool Dispatch()
        {
            if (sleeveDispatcher != null)
            {
                if (sleeveDispatcher.Process())
                {
                    Debug.Log("Sleeve Cards Dispatched");
                    sleeveDispatcher = null;
                }
                else
                    return false;
            }

            if (handDispatcher != null)
			{
				if (handDispatcher.Process ())
                {
                    Debug.Log("Hand Cards Dispatched");
                    handDispatcher = null;
                }
				else
					return false;
			}

            if (allHandCardsLayoutUpdater != null)
            {
                if (allHandCardsLayoutUpdater.Process(true))
                {
                    Debug.Log("Hand Cards Layout Updated");
                    allHandCardsLayoutUpdater = null;
                }
                else
                    return false;
            }

            if (userSleeveCardsLayoutUpdater != null)
            {
                if (userSleeveCardsLayoutUpdater.Process(true))
                {
                    Debug.Log("Sleeve Cards Layout Updated");
                    userSleeveCardsLayoutUpdater = null;
                }
                else
                    return false;
            }

            if (userHandCardsOpener != null)
			{
				if (userHandCardsOpener.Process (true))
                {
                    Debug.Log("User Hand Cards Opened");
                    userHandCardsOpener = null;
                }
				else
					return false;
			}

            if (userSleeveCardsOpener != null)
            {
                if (userSleeveCardsOpener.Process(true))
                {
                    Debug.Log("User Sleeve Cards Opened");
                    userSleeveCardsOpener = null;
                }
				else
					return false;
			}

            return true;
		}

		private bool Thinking()
		{
            if (!isThinking)
            {
                EventManager.Attach<EndThinkingEvent>(OnThinkingEndCallback);
                EventManager.Send<StartThinkingEvent>(new StartThinkingEvent());

                isThinking = true;
                isWaitingForEndThinking = true;
            }

            return !isWaitingForEndThinking;
		}

		private bool Opening()
		{
			if (allHandCardsOpener != null)
			{
				if (allHandCardsOpener.Process (true))
					allHandCardsOpener = null;
				else
					return false;
			}

			return true;
		}

		private bool ScoreReward(Player winner)
		{
			if (!isWinnerScoreRewarded)
            {
                CreateRetrievers();

                if (winner != null)
                {
                    winner.AddScore(data.ScoreReward);

                    winnerCardsHighlighter = new HandCardsHighlighter();
                    winnerCardsHighlighter.Configure(new List<Player>() { winner }, 1);

                    isWinnerScoreRewarded = true;

                    Debug.Log("Reward Dispatched");

                    return false;
                }
            }

            if (winnerCardsHighlighter != null)
            {
                if (winnerCardsHighlighter.Process(true))
                {
                    Debug.Log("Winner Cards Highlighted");
                    winnerCardsHighlighter = null;
                }
                else
                    return false;
            }

            if (allHandCardsOpener != null)
            {
                if (allHandCardsOpener.Process(false))
                {
                    Debug.Log("All Cards Closed");
                    allHandCardsOpener = null;
                }
                else
                    return false;
            }

            if (allHandCardsRetriever != null)
            {
                if (allHandCardsRetriever.Process())
                {
                    Debug.Log("All Hand Cards Retrieved");
                    allHandCardsRetriever = null;
                }
                else
                    return false;
            }

            if (userSleeveCardsRetriever != null)
            {
                if (userSleeveCardsRetriever.Process())
                {
                    Debug.Log("All Sleeve Cards Retrieved");
                    userSleeveCardsRetriever = null;
                }
                else
                    return false;
            }

			return true;
		}

		#endregion

		private bool CheckEndOfGame(Player winner, Player loser)
		{
			if (loser == null && winner == null)
				return true;

			GamesManager.GameEndEvent e = new GamesManager.GameEndEvent ();

			if (loser != null && UserController.Instance.GetPlayer () == loser)
			{
				e.targetPlayer = loser;
				e.result = GamesManager.Result.Lose;
				e.extra.Add (EXTRA_PHASE, phase);

				EventManager.Send<GamesManager.GameEndEvent>(e);
				return false;
			}

			if (winner != null)
			{
				e.targetPlayer = winner;
				e.result = GamesManager.Result.Win;
				e.extra.Add (EXTRA_SCORE_REWARD, data.ScoreReward);

                EventManager.Send<GamesManager.GameEndEvent>(e);
				return true;
			}

			return true;
		}

		private void OnThinkingEndCallback(EndThinkingEvent e)
		{
			EventManager.Detach<EndThinkingEvent> (OnThinkingEndCallback);

            isWaitingForEndThinking = false;
		}

		#region Configure Methods

		private void CreatePlayers(int count)
		{
			List<Player> players = null;

			if (BoardController.Instance.GetPlayerCount () > 0)
			{
				players = new List<Player> (BoardController.Instance.GetPlayers ());
                Debug.Log("Using old players");
			}
			else
            {
                Debug.Log("Creating new players");
                players = new List<Player>();

                // user setup
                var sharper = BoardController.CreateSharper();
                sharper.Configure(data.UserPlayerId);

                BoardController.Instance.AddPlayer(sharper);
                players.Add(sharper);

                UserController.Instance.SetPlayer(sharper);

				// enemy players setup
				for (int i = 0; i < count; i++)
				{
					var player = BoardController.CreatePlayer ();

					player.Configure (data.PlayersDB.GetUnused ().Id);

					BoardController.Instance.AddPlayer (player);
					players.Add (player);
				}
			}

			CreateDispatchers (players.ToArray ());

            EventManager.Send<GamesManager.GameStartedEvent>(new GamesManager.GameStartedEvent() { players = players });
        }

		private void CreateDispatchers(Player[] players)
		{
			Player[] sharperPlayers = Array.FindAll (players, p => p is Sharper);
			var sharpers = Array.ConvertAll (sharperPlayers, p => (Sharper)p);

            handDispatcher = new HandCardsDispatcher();
			handDispatcher.Configure (players, players.Length, data.HandCardsCount);

            sleeveDispatcher = new SleeveCardsDispatcher (data.Template);
			sleeveDispatcher.Configure (sharpers, sharpers.Length, data.SleeveCardsCount);

            allHandCardsLayoutUpdater = new HandCardsLayoutUpdater();
            allHandCardsLayoutUpdater.Configure(players, players.Length);

            userSleeveCardsLayoutUpdater = new SleeveCardsLayoutUpdater();
            userSleeveCardsLayoutUpdater.Configure(new List<Sharper>() { (Sharper)UserController.Instance.GetPlayer() }, 1);

            allHandCardsOpener = new HandCardsOpener ();
			allHandCardsOpener.Configure (players, players.Length);

            userHandCardsOpener = new HandCardsOpener ();
            userHandCardsOpener.Configure(new List<Player> () { UserController.Instance.GetPlayer () }, 1);

            userSleeveCardsOpener = new SleeveCardsOpener ();
            userSleeveCardsOpener.Configure(new List<Sharper> () { (Sharper)UserController.Instance.GetPlayer () }, 1);
		}

        private void CreateRetrievers()
        {
            allHandCardsOpener = new HandCardsOpener();
            allHandCardsOpener.Configure(BoardController.Instance.GetPlayers(), BoardController.Instance.GetPlayerCount());

            allHandCardsRetriever = new HandCardsRetriever();
            allHandCardsRetriever.Configure(BoardController.Instance.GetPlayers(), BoardController.Instance.GetPlayerCount(), data.HandCardsCount);

            userSleeveCardsRetriever = new SleeveCardsRetriever();
            userSleeveCardsRetriever.Configure(new List<Sharper>() { (Sharper)UserController.Instance.GetPlayer() }, 1, data.SleeveCardsCount);
        }

		#endregion
	}
}

