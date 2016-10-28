using System;
using UnityEngine;


namespace Cards
{
    public class LeanTweenCardAnimator : MonoBehaviour, ICardAnimator
    {
        [SerializeField]
        private LeanTweenType       moveTween;

        [SerializeField]
        private float               moveTime;
        
        [SerializeField]
        private LeanTweenType       openCloseTween;

        [SerializeField]
        private float               openCloseTime;

        [SerializeField]
        private Vector3             openCloseAxis;

        [SerializeField]
        private float               openCloseAngle;

        [SerializeField]
        private LeanTweenType       popPushTween;

        [SerializeField]
        private float               popPushTime;

        [SerializeField]
        private Vector3             popPushOffset;

        [SerializeField]
        private LeanTweenType       highlightTween;

        [SerializeField]
        private float               highlightTime;

        [SerializeField]
        private Vector3             highlightPosition;

		private ICardTemplate       target;


		public void SetTarget(ICardTemplate target)
        {
            this.target = target;
        }

        public void Move(Vector3 position, Action onCompleteCallback = null)
        {
            if (target == null)
                return;

			LeanTween.move(target.GetObject (), position, moveTime)
                .setEase(moveTween)
                .setOnComplete(onCompleteCallback != null ? onCompleteCallback : () => { });
        }

        public void Open(Action onCompleteCallback = null)
        {
            if (target == null)
                return;

			LeanTween.rotateAroundLocal(target.GetObject (), openCloseAxis, openCloseAngle, openCloseTime)
                .setEase(openCloseTween)
                .setOnComplete(onCompleteCallback != null ? onCompleteCallback : () => { });
        }

        public void Close(Action onCompleteCallback = null)
        {
            if (target == null)
                return;

			LeanTween.rotateAroundLocal(target.GetObject (), openCloseAxis, -openCloseAngle, openCloseTime)
                .setEase(openCloseTween)
                .setOnComplete(onCompleteCallback != null ? onCompleteCallback : () => { });
        }

        public void Pop(Action onCompleteCallback = null)
        {
            if (target == null)
                return;

			LeanTween.move(target.GetObject (), target.GetObject ().transform.position + popPushOffset, popPushTime)
                .setEase(popPushTween)
                .setOnComplete(onCompleteCallback != null ? onCompleteCallback : () => { });
        }

        public void Push(Action onCompleteCallback = null)
        {
            if (target == null)
                return;

			LeanTween.move(target.GetObject (), target.GetObject ().transform.position + popPushOffset * -1, popPushTime)
                .setEase(popPushTween)
                .setOnComplete(onCompleteCallback != null ? onCompleteCallback : () => { });
        }

        public void Highlight(Action onCompleteCallback = null)
        {
            if (target == null)
                return;

            HighlightLopped(false, () => HighlightLopped(true, onCompleteCallback));
        }

        private void HighlightLopped(bool isIn, Action onDoneCallback)
        {
            Vector3 pos = isIn ? target.GetObject().transform.position + highlightPosition : target.GetObject().transform.position - highlightPosition;

            LeanTween.move(target.GetObject(), pos, highlightTime)
                .setEase(highlightTween)
                .setOnComplete(onDoneCallback);
        }
    }
}
