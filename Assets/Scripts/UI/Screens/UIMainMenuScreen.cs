using UnityEngine;
using Games;
using UnityEngine.UI;


namespace UI.Screens
{
	public class UIMainMenuScreen : UIScreen 
	{
		private int playersCount;

		[SerializeField]
		private Text playersCountLabel;


		public override void Init ()
		{
			base.Init ();

			playersCount = GamesManager.Instance.GetDeadOfKingsMinPlayers ();
			playersCountLabel.text = string.Format ("{0}", playersCount);
		}

        public void OnIncrementPlayersCountClicked()
		{
			if (playersCount + 1 <= GamesManager.Instance.GetDeadOfKingsMaxPlayers ())
				playersCountLabel.text = string.Format ("{0}", ++playersCount);
		}

		public void OnDecrementPlayersCountClicked()
		{
			if (playersCount - 1 >= GamesManager.Instance.GetDeadOfKingsMinPlayers ())
				playersCountLabel.text = string.Format ("{0}", --playersCount);
		}

		public void OnStartGameClicked()
		{
			EventManager.Send<GamesManager.GameStartEvent> (
				new GamesManager.GameStartEvent() { playersCount = playersCount }
			);

			UIController.Instance.Show<UIHUDScreen> ();
		}
	}
}
