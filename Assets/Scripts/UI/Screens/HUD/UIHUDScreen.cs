using System.Collections.Generic;
using UnityEngine;

using Games;
using Utils;
using Controllers;

namespace UI.Screens
{
	public class UIHUDScreen : UIScreen
	{
		[SerializeField]
		private GameObject playerLabelPrototype;

		[SerializeField]
		private Transform playerLabelsContainer;

        [SerializeField]
        private GameObject openingButton;

        [SerializeField]
        private GameObject swapButton;

        private List<PlayerLabel> labels;


		public override void Init ()
		{
			base.Init ();

            labels = new List<PlayerLabel>();

            openingButton.SetActive(false);
            swapButton.SetActive(false);

            EventManager.Attach<GamesManager.GameStartedEvent>(OnGameStarted);
            EventManager.Attach<GamesManager.GameEndEvent>(OnGameEnd);

            EventManager.Attach<DeadOfKingsGame.StartThinkingEvent>(OnStartThinking);
            EventManager.Attach<UserController.SwapCardsStartEvent>(OnSwapCardsStartCallback);
        }

		public override void DeInit ()
		{
			base.DeInit ();

            EventManager.Detach<GamesManager.GameStartedEvent>(OnGameStarted);
            EventManager.Detach<GamesManager.GameEndEvent>(OnGameEnd);

            EventManager.Detach<DeadOfKingsGame.StartThinkingEvent>(OnStartThinking);
            EventManager.Detach<UserController.SwapCardsStartEvent>(OnSwapCardsStartCallback);

            labels.Clear();
            labels = null;

        }

		private void OnGameStarted(GamesManager.GameStartedEvent e)
		{
            ClearLabels();

            foreach (var player in e.players)
            {
                var label = CreateLabel(playerLabelPrototype);
                var pd = GamesManager.Instance.GetDeadOfKingsDatabase().Get(player.GetId());

                label.Configure(pd.Id, pd.Icon, pd.FirstName, pd.LastName, player.GetScore());
                labels.Add(label);
            }
		}

        private void OnGameEnd(GamesManager.GameEndEvent e)
        {
            if (e.result == GamesManager.Result.Win)
            {
                var lbl = labels.FindLast(l => l.Id == e.targetPlayer.GetId());
                if (lbl != null)
                {
                    lbl.UpdateScore(e.targetPlayer.GetScore());
                    lbl.Win();
                }
            }
            else
            {
                UIController.Instance.GetScreen<UIGameResultScreen>().OnGameEndCallback(e);
                UIController.Instance.Show<UIGameResultScreen>();
            }
        }

        private PlayerLabel CreateLabel(GameObject prototype)
        {
            GameObject labelGO = Pool.Instance.Get(prototype);

            labelGO.transform.SetParent(playerLabelsContainer);

            labelGO.transform.localPosition = Vector3.zero;
            labelGO.transform.localRotation = Quaternion.identity;
            labelGO.transform.localScale = Vector3.one;

            labelGO.gameObject.SetActive(true);
            return labelGO.GetComponent<PlayerLabel>();
        }

        private void ClearLabels()
        {
            foreach (var label in labels)
                Pool.Instance.Retrieve(label.gameObject);

            labels.Clear();
        }

        private void OnStartThinking(DeadOfKingsGame.StartThinkingEvent e)
        {
            openingButton.SetActive(true);
        }

        private void OnSwapCardsStartCallback(UserController.SwapCardsStartEvent e)
        {
            swapButton.SetActive(true);
        }

        public void OnOpeningClicked()
        {
            openingButton.SetActive(false);
            EventManager.Send<DeadOfKingsGame.EndThinkingEvent>(new DeadOfKingsGame.EndThinkingEvent());
        }

        public void OnSwapCards()
        {
            swapButton.SetActive(false);
            EventManager.Send<UserController.SwapCardsEndEvent>(new UserController.SwapCardsEndEvent());
        }
    }
}

