using UI.Screens;
using UnityEngine;


namespace UI.Common
{
    public class UIAnimator : MonoBehaviour
    {
        protected UIScreen screen;

        protected virtual void Awake()
        {
            screen = GetComponent<UIScreen>();

            screen.OnShow           += OnShow;
            screen.OnShowComplete   += OnShowComplete;

            screen.OnHide           += OnHide;
            screen.OnHideComplete   += OnHideComplete;
        }

        protected virtual void OnDestroy()
        {
            screen.OnShow           -= OnShow;
            screen.OnShowComplete   -= OnShowComplete;

            screen.OnHide           -= OnHide;
            screen.OnHideComplete   -= OnHideComplete;

            screen = null;
        }

        public virtual void OnShow()
        {

        }

        public virtual void OnShowComplete()
        {

        }

        public virtual void OnHide()
        {

        }

        public virtual void OnHideComplete()
        {

        }
    }
}
