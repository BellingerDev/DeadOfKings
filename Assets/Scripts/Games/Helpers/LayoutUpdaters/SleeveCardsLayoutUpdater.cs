using Players;


namespace Games.Helpers
{
    public class SleeveCardsLayoutUpdater : PerPlayerProcessor<Sharper>
    {
        private Player targetPlayer;


        protected override void Open(Sharper player)
        {
            //if (targetPlayer == null)
            //{
            //    targetPlayer = player;
            //    targetPlayer.UpdateFanLayout();
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
