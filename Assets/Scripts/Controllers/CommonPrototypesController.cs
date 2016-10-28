using UnityEngine;

using Utils;


namespace Controllers
{
    public class CommonPrototypesController : SingletonMonoBehaviour<CommonPrototypesController>
    {
        [SerializeField]
        private GameObject card;

        [SerializeField]
        private GameObject player;

        [SerializeField]
        private GameObject sharper;


        public GameObject Card { get { return card; } }
        public GameObject Player { get { return player; } }
        public GameObject Sharper { get { return sharper; } }
    }
}
