namespace UI.Common
{
    public class UIElementDisabler : UIAnimator
    {
        public override void OnHideComplete()
        {
            this.gameObject.SetActive(false);
        }
    }
}
