using UnityEngine;
using System.Linq;

using Players;
using Cards;
using Games;
using Utils;


namespace Controllers
{
	public class UserController : SingletonMonoBehaviour<UserController>
	{
        #region Events

        public class SwapCardsStartEvent : EventManager.IEvent { }
        public class SwapCardsEndEvent : EventManager.IEvent { }

        #endregion

        [SerializeField]
        private LayerMask raycastLayers;

        [SerializeField]
        private float raycastMaxDistance;

		private Sharper sharper;

		private Card selectedHandCard;
		private Card selectedSleeveCard;

        private bool isThinking;

        private bool isEndSwapHand;
        private bool isEndSwapSleeve;


        protected override void Awake()
        {
            base.Awake();

            isEndSwapHand = false;
            isEndSwapSleeve = false;

            EventManager.Attach<DeadOfKingsGame.StartThinkingEvent>(OnStartThinkingCallback);
            EventManager.Attach<DeadOfKingsGame.EndThinkingEvent>(OnEndThinkingCallback);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            EventManager.Detach<DeadOfKingsGame.StartThinkingEvent>(OnStartThinkingCallback);
            EventManager.Detach<DeadOfKingsGame.EndThinkingEvent>(OnEndThinkingCallback);
        }

        public void SetPlayer(Player player)
		{
			sharper = player as Sharper;
		}

		public Player GetPlayer()
		{
			return sharper;
		}

		public void SelectHandCard(Card card)
		{
			selectedHandCard = card;
			card.Pop ();
		}

		public void SelectSleeveCard(Card card)
		{
			selectedSleeveCard = card;
			card.Pop ();
		}

		public void SwapCards()
		{
            if (selectedHandCard == null || selectedSleeveCard == null)
                return;

            sharper.RemoveCardFromHand(selectedHandCard);
            sharper.RemoveCardFromSleeve(selectedSleeveCard);

            Vector3 cardHandPos = selectedHandCard.GetTemplate().GetObject().transform.position;
            Vector3 cardSleevePos = selectedSleeveCard.GetTemplate().GetObject().transform.position;

            selectedHandCard.Move(cardSleevePos, () =>
            {
                sharper.AddCardToSleeve(selectedHandCard);
                selectedHandCard.Push();

                isEndSwapHand = true;
            });

            selectedSleeveCard.Move(cardHandPos, () =>
            {
                sharper.AddCardToHand(selectedSleeveCard);
                selectedSleeveCard.Push();

                isEndSwapSleeve = true;
            });
		}

        private void Update()
        {
            if (isEndSwapHand && isEndSwapSleeve)
            {
                isEndSwapHand = false;
                isEndSwapSleeve = false;

                selectedHandCard = null;
                selectedSleeveCard = null;

                sharper.UpdateFanLayout();
            }

            if (isThinking)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = CameraController.Instance.GameCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, raycastMaxDistance, raycastLayers))
                    {
                        ICardTemplate template = hit.collider.GetComponent<ICardTemplate>();

                        if (template != null)
                        {
                            Card card = sharper.GetHandCards().SingleOrDefault(c => c.GetTemplate() == template);

                            if (card != null)
                            {
                                if (selectedHandCard != null && selectedHandCard != card)
                                    selectedHandCard.Push();

                                if (card != selectedHandCard)
                                    card.Pop();

                                selectedHandCard = card;
                            }

                            card = sharper.GetSleeveCards().SingleOrDefault(c => c.GetTemplate() == template);

                            if (card != null)
                            {
                                if (selectedSleeveCard != null && selectedSleeveCard != card)
                                    selectedSleeveCard.Push();

                                if (card != selectedSleeveCard)
                                    card.Pop();

                                selectedSleeveCard = card;
                            }
                        }
                    }

                    if (selectedHandCard != null && selectedSleeveCard != null)
                        EventManager.Send<SwapCardsStartEvent>(new SwapCardsStartEvent());
                }
            }
        }

        private void OnStartThinkingCallback(DeadOfKingsGame.StartThinkingEvent e)
        {
            isThinking = true;
            EventManager.Attach<SwapCardsEndEvent>(OnSwapCardsEndCallback);
        }

        private void OnEndThinkingCallback(DeadOfKingsGame.EndThinkingEvent e)
        {
            isThinking = false;
            EventManager.Detach<SwapCardsEndEvent>(OnSwapCardsEndCallback);
        }

        private void OnSwapCardsEndCallback(SwapCardsEndEvent e)
        {
            SwapCards();
        }
	}
}

