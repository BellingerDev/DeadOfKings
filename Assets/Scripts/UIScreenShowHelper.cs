using UnityEngine;

using UI;
using UI.Screens;


public class UIScreenShowHelper : MonoBehaviour
{
    private enum ScreenType
    {
        MainMenu,
        HUD
    }

    [SerializeField]
    private ScreenType type;

    [SerializeField]
    private float showDelay;

    private float showTime;
    public bool IsShowed { get; set; }

    private void Awake()
    {
        IsShowed = false;
        showTime = Time.time + showDelay;
    }

    private void Update()
    {
        if (!IsShowed && Time.time > showTime)
        {
            switch (type)
            {
                case ScreenType.MainMenu:
                    UIController.Instance.Show<UIMainMenuScreen>();
                    break;

                case ScreenType.HUD:
                    UIController.Instance.Show<UIHUDScreen>();
                    break;
            }

            IsShowed = true;
        }
    }
}
