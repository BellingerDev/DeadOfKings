using UI.Screens;
using UnityEngine;


namespace UI.Common
{
    public class UIAnimateShift : UIAnimator
    {
        [SerializeField]
        private Vector3 shift;

        [SerializeField]
        private float time;

        [SerializeField]
        private LeanTweenType tween;

        private Vector3 origin;


        protected override void Awake()
        {
            base.Awake();
            origin = this.transform.localPosition;
        }

        public override void OnShow()
        {
            this.transform.localPosition = origin + shift;

            LeanTween.moveLocal(this.gameObject, origin, time)
                .setEase(tween)
                .setOnComplete(() =>
                {
                    screen.RaiseAction(screen.OnShowComplete);
                });
        }

        public override void OnHide()
        {
            this.transform.localPosition = origin;

            LeanTween.moveLocal(this.gameObject, origin + shift, time)
                .setEase(tween)
                .setOnComplete(() =>
                {
                    screen.RaiseAction(screen.OnHideComplete);
                });
        }
    }
}
