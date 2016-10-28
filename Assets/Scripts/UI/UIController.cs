using System;
using UI.Screens;
using UnityEngine;


namespace UI
{
    public class UIController : MonoBehaviour
    {
        #region Singleton Realize
        private static WeakReference instance;
        public static UIController Instance
        {
            get
            {
                if (instance == null || instance.Target == null)
                    instance = new WeakReference(FindObjectOfType<UIController>());
            
                return instance.Target as UIController;
            }
        }
        #endregion

        private UIScreen[] screens;


        public void Init()
        {
            screens = GetComponentsInChildren<UIScreen>(true);


            foreach (var s in screens)
            {
                s.Init();
                s.gameObject.SetActive(false);
            }
        }

        public void DeInit()
        {
            foreach (var s in screens)
            {
                s.DeInit();
            }

            HideAll();

            screens = null;
        }

        public void Show<T>() where T : UIScreen
        {
            HideAll();

            UIScreen screen = Array.FindLast<UIScreen>(screens, s => s is T);
            if (screen != null)
                screen.Show();

            Debug.Log("UIControler : Screen : " + typeof(T).ToString() + " was showed");
        }

        public T GetScreen<T>() where T : UIScreen
        {
            return (T)Array.FindLast<UIScreen>(screens, s => s is T);
        } 

        private void HideAll()
        {
            foreach (var s in screens)
                s.Hide();
        }
    }
}
