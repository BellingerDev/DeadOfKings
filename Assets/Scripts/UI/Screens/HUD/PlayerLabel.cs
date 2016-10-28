using UnityEngine;
using UnityEngine.UI;


namespace UI.Screens
{
    public class PlayerLabel : MonoBehaviour
    {
        [SerializeField]
        private Image icon;

        [SerializeField]
        private Text fName;

        [SerializeField]
        private Text lName;

        [SerializeField]
        private Text score;

        [SerializeField]
        private GameObject winMarker;

        [SerializeField]
        private float winMarkerDelay;

        private float timeToHideMarker;
        private bool isWin;

        public string Id { get; private set; }


        public void Configure(string id, Sprite icon, string fName, string lName, int score)
        {
            this.Id = id;
            this.icon.sprite = icon;
            this.fName.text = fName;
            this.lName.text = lName;
            this.score.text = score.ToString();

            isWin = false;
            winMarker.SetActive(false);
        }

        public void Win()
        {
            winMarker.SetActive(true);
            isWin = true;
            timeToHideMarker = Time.time + winMarkerDelay;
        }

        private void Update()
        {
            if (isWin && Time.time > timeToHideMarker)
            {
                winMarker.SetActive(false);
                isWin = false;
            }
        }

		public void UpdateScore(int score)
		{
			this.score.text = score.ToString ();
		}
	}
}
