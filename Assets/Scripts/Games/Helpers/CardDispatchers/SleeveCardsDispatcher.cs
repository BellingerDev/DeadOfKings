using UnityEngine;

using Players;
using Controllers;
using Cards;


namespace Games.Helpers
{
	public class SleeveCardsDispatcher : PerCardProcessor<Sharper>
	{
		private ICardTemplate template;
        private Card cardToDispatch;

		public SleeveCardsDispatcher(ICardTemplate template) 
		{
			this.template = template;
		}

		protected override void AssignCard (Sharper player)
		{
            if (cardToDispatch == null)
            {
                cardToDispatch = DeckController.Instance.GenerateRandomCard(Card.State.Closed, template);
                cardToDispatch.GetTemplate().GetObject().transform.position = player.transform.position + new Vector3(0, 10, 0);
                cardToDispatch.Move(player.transform.position + new Vector3(5, 0, 0), () => {
                    player.AddCardToSleeve(cardToDispatch);
                    cardToDispatch = null;
                    base.AssignCard(player);
                });
            }
		}
	}
}
