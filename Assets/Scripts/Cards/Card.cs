using UnityEngine;
using System;
using Random = UnityEngine.Random;


namespace Cards
{
    public class Card : MonoBehaviour, IComparable<Card>
    {
        #region Enums

        public enum Suit
        {
			Clubs = 0, Diamonds, Spades, Hearts, 
        }

        public enum Rank
        {
            Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
        }

        public enum State
        {
            Opened = 0, Closed
        }

        #endregion

        private Suit            suit;
        private Rank            rank;
        private State           state;

        private ICardAnimator 	animator;

		private ICardTemplate  	template;


        #region UnityEvents

        private void Awake()
        {
            animator = GetComponent<ICardAnimator>();
        }

        private void OnDestroy()
        {
            animator = null;
        }

        #endregion

		public void Configure(Suit suit, Rank rank, State state, ICardTemplate template)
        {
            this.suit = suit;
            this.rank = rank;
            this.state = state;
            this.template = template;

            this.template.Configure(suit, rank, state);

			animator.SetTarget(this.template);
        }

        public int CompareTo(Card other)
        {
            return Random.Range(0, 1);
        }

        #region Getters and Setters

        public Suit GetSuit()
        {
            return suit;
        }

        public Rank GetRank()
        {
            return rank;
        }

		public ICardTemplate GetTemplate()
		{
			return template;
		}

		#endregion

        #region Manipulation Methods

		public void Move(Vector3 position, Action onDoneCallback = null)
        {
			animator.Move(position, onDoneCallback);
        }

        public void Open(Action onDoneCallback = null)
        {
            if (state == State.Opened)
            {
                if (onDoneCallback != null)
                    onDoneCallback();

                return;
            }

            animator.Open(onDoneCallback);
            state = State.Opened;
        }

        public void Close(Action onDoneCallback = null)
        {
            if (state == State.Closed)
            {
                if (onDoneCallback != null)
                    onDoneCallback();

                return;
            }

            animator.Close(onDoneCallback);
            state = State.Closed;
        }

        public void Pop()
        {
            animator.Pop();
        }

        public void Push()
        {
            animator.Push();
        }

        public void Hightlight(Action onDoneCallback)
        {
            animator.Highlight(onDoneCallback);
        }

        #endregion
    }
}
