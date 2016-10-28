namespace UI.Common
{
    public class UIAnimateDummy : UIAnimator
    {
        public override void OnHide()
        {
            screen.RaiseAction(screen.OnHideComplete);
        }
    }
}
