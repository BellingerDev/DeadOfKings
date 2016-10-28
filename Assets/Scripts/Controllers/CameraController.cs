using UnityEngine;
using Utils;


namespace Controllers
{
    public class CameraController : SingletonMonoBehaviour<CameraController>
    {
        public Camera GameCamera { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            GameCamera = GetComponent<Camera>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            GameCamera = null;
        }
    }
}
