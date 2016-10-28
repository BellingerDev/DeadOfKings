using System;
using UnityEngine;


namespace Utils
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour 
        where T : MonoBehaviour
    {
        private static WeakReference instance;
        public static T Instance
        {
            get
            {
                if (instance == null || instance.Target == null)
					Debug.Log("SingletonGO not found !!!");

                return instance.Target as T;
            }
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = new WeakReference(this);
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }

        protected SingletonMonoBehaviour() { }
    }
}
