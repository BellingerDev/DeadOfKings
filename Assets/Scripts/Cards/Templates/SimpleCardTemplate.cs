using UnityEngine;
using System;


namespace Cards
{
	public class SimpleCardTemplate : MonoBehaviour, ICardTemplate
	{
		private const string DIFFUSE_TEXTURE_ID = "_Diffuse";

		[SerializeField]
		private Vector2 facesAtlasSize;

		[SerializeField]
		private Vector2 faceSize;

		[SerializeField]
		private MeshRenderer face;

        #region ICardTemplate implementation

        public void Configure (Card.Suit suit, Card.Rank rank, Card.State state)
		{
			int suitsCount = Enum.GetValues (typeof(Card.Suit)).Length;
			int ranksCount = Enum.GetValues (typeof(Card.Rank)).Length;
            
			Vector2 faceScale = new Vector2 (faceSize.x / facesAtlasSize.x, faceSize.y / facesAtlasSize.y);
			Vector2 faceOffset = new Vector2 (
				                     (faceScale.x * (float)rank), 
				                     (faceScale.y * (float)suit)
			                     );

			face.material.SetTextureScale (DIFFUSE_TEXTURE_ID, faceScale);
			face.material.SetTextureOffset (DIFFUSE_TEXTURE_ID, faceOffset);
            
            if (state == Card.State.Closed)
                ForceFlip();
		}

		public void ForceFlip ()
		{
            this.transform.Rotate(Vector3.forward, 180);
		}

		public GameObject GetObject ()
		{
			return this.gameObject;
		}

		#endregion
		
	}
}

