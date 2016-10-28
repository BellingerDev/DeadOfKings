using System.Collections.Generic;

using Cards;
using Players;


namespace Games.Helpers
{
	public class HandCardsOpener : PerPlayerProcessor<Player>
	{
        Queue<Card> cardsToProcess;
        private bool isLocked = false;


        protected override void Open (Player player)
		{
            if (cardsToProcess == null)
                cardsToProcess = new Queue<Card>(player.GetHandCards());

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

		protected override void Close (Player player)
		{
            if (cardsToProcess == null)
                cardsToProcess = new Queue<Card>(player.GetHandCards());

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

