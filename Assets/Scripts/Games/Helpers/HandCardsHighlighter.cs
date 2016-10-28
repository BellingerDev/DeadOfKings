using System.Collections.Generic;

using Cards;
using Players;


namespace Games.Helpers
{
    public class HandCardsHighlighter : PerPlayerProcessor<Player>
    {
        private Queue<Card> cardsToHightlight;
        private bool isLock;


        protected override void Open(Player player)
        {
            if (cardsToHightlight == null)
            {
                cardsToHightlight = new Queue<Card>(player.GetHandCards());
                isLock = false;
            }

            if (!isLock && cardsToHightlight.Count > 0)
            {
                isLock = true;

                cardsToHightlight.Peek().Hightlight(() =>
                {
                    cardsToHightlight.Dequeue();
                    isLock = false;
                });
            }

            if (cardsToHightlight.Count == 0)
            {
                cardsToHightlight = null;
                base.Open(player);
            }
        }
    }
}
