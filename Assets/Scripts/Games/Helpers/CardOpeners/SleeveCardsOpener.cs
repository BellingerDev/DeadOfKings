using Cards;
using Players;
using System.Collections.Generic;


namespace Games.Helpers
{
	public class SleeveCardsOpener : PerPlayerProcessor<Sharper>
    {
        Queue<Card> cardsToProcess;
        private bool isLocked = false;

        protected override void Open (Sharper player)
		{
            if (cardsToProcess == null)
                cardsToProcess = new Queue<Card>(player.GetSleeveCards());

            if (!isLocked && cardsToProcess.Count > 0)
            {
                isLocked = true;

                cardsToProcess.Peek().Open(() => {
                    cardsToProcess.Dequeue();
                    isLocked = false;
                });
            }

            if (cardsToProcess.Count == 0)
            {
                cardsToProcess = null;
                base.Open(player);
            }
        }

		protected override void Close (Sharper player)
		{
            if (cardsToProcess == null)
                cardsToProcess = new Queue<Card>(player.GetSleeveCards());

            if (!isLocked && cardsToProcess.Count > 0)
            {
                isLocked = true;

                cardsToProcess.Peek().Close(() => {
                    cardsToProcess.Dequeue();
                    isLocked = false;
                });
            }

            if (cardsToProcess.Count == 0)
            {
                cardsToProcess = null;
                base.Close(player);
            }
        }
	}
}

