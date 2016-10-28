using System.Collections.Generic;
using UnityEngine;

using Players;
using Cards;
using Utils;


namespace Games.Helpers
{
    public class SleeveCardsRetriever : PerCardProcessor<Sharper>
    {
        private Queue<Card> cardsToReterieve;
        private bool isLock;


        protected override void AssignCard(Sharper player)
        {
            if (cardsToReterieve == null)
            {
                cardsToReterieve = new Queue<Card>(player.GetSleeveCards());
                isLock = false;
            }

            if (!isLock && cardsToReterieve.Count > 0)
            {
                isLock = true;

                cardsToReterieve.Peek().Move(player.transform.position + new Vector3(0, 10, 0), () =>
                {
                    Card card = cardsToReterieve.Dequeue();

                    Pool.Instance.Retrieve(card.GetTemplate().GetObject());
                    Pool.Instance.Retrieve(card.gameObject);

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
