using System;
using UI.Common;
using UnityEngine;


namespace UI.Screens
{
    public abstract class UIScreen : MonoBehaviour
    {
        public enum ScreenState
        {
            Showed,
            Hidden
        }

        public Action OnShow { get; set; }
        public Action OnShowComplete { get; set; }

        public Action OnHide { get; set; }
        public Action OnHideComplete { get; set; }

        public ScreenState State { get; private set; }


        public virtual void Init()
        {
            State = ScreenState.Hidden;
        }

        public virtual void DeInit()
        {

        }

        public virtual void Show()
        {
            if (State == ScreenState.Showed)
                return;

            State = ScreenState.Showed;
            this.gameObject.SetActive(true);
            RaiseAction(OnShow);
        }

        public virtual void Hide()
        {
            if (State == ScreenState.Hidden)
                return;

            State = ScreenState.Hidden;
            RaiseAction(OnHide);
        }

        public void RaiseAction(Action action)
        {
            if (action != null)
                action();
        }
    }
}
