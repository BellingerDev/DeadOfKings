using System.Collections.Generic;

using Players;
using Cards;
using Controllers;


namespace Games.Helpers
{
    public class HandCardsRetriever : PerCardProcessor<Player>
    {
        private Queue<Card> cardsToReterieve;
        private bool isLock;


        protected override void AssignCard(Player player)
        {
            if (cardsToReterieve == null)
            {
                cardsToReterieve = new Queue<Card>(player.GetHandCards());
                isLock = false;
            }

            if (!isLock && cardsToReterieve.Count > 0)
            {
                isLock = true;

                cardsToReterieve.Peek().Move(DeckController.Instance.transform.position, () =>
                {
                    player.RemoveCardFromHand(cardsToReterieve.Peek());
                    DeckController.Instance.RetrieveCard(cardsToReterieve.Dequeue());

                    isLock = false;
                });
            }

            if (cardsToReterieve.Count == 0)
            {
                cardsToReterieve = null;
                base.AssignCard(player);
            }
        }
    }
}
