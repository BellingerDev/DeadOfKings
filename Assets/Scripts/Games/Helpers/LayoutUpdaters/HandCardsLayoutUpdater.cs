using Players;


namespace Games.Helpers
{
    public class HandCardsLayoutUpdater : PerPlayerProcessor<Player>
    {
        private Player targetPlayer;


        protected override void Open(Player player)
        {
            //if (targetPlayer == null)
            //{
            //    targetPlayer = player;
            //    targetPlayer.UpdateFanLayout();

            //    UnityEngine.Debug.Log("Start Update Layout of player : " + player.GetId());
            //}

            //if (!targetPlayer.IsFanLayoutUpdating())
            //{
            //    targetPlayer = null;
            //    base.Open(player);
            //}

            player.UpdateFanLayout();
            base.Open(player);
        }
    }
}
