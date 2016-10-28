using System;
using UnityEngine;


namespace Cards
{
    public interface ICardAnimator
    {
		void SetTarget(ICardTemplate target);

        void Move(Vector3 position, Action onCompleteCallback = null);

        void Open(Action onCompleteCallback = null);
        void Close(Action onCompleteCallback = null);

        void Pop(Action onCompleteCallback = null);
        void Push(Action onCompleteCallback = null);

        void Highlight(Action onCompleteCallback = null);
    }
}
