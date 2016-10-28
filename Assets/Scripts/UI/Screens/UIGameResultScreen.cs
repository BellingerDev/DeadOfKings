using Controllers;
using Games;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Screens
{
    public class UIGameResultScreen : UIScreen
    {
        [SerializeField]
        private Text messageText;

        [SerializeField]
        private string playerDeadMessage;

        [SerializeField]
        private string enemyCardDublicatesFoundMessage;


        public void OnGameEndCallback(GamesManager.GameEndEvent e)
        {
            if (e.result == GamesManager.Result.Lose)
            {
                UIController.Instance.Show<UIGameResultScreen>();

                if (UserController.Instance.GetPlayer() != e.targetPlayer)
                    messageText.text = enemyCardDublicatesFoundMessage;
                else
                    messageText.text = playerDeadMessage;
            }
        }

        public void OnRestartClicked()
        {
            EventManager.Send<GamesManager.GameStartEvent>(new GamesManager.GameStartEvent());
        }
    }
}
