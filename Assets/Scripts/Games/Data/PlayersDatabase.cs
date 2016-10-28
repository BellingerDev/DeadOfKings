using System;
using UnityEngine;
using System.Collections.Generic;


namespace Games.Data
{
	[CreateAssetMenu(fileName = "PlayersDatabase", menuName = "Data/PlayersDatabase")]
	public class PlayersDatabase : ScriptableObject
	{
		private const string DATABASE_USED_IDS = "DatabaseUsedIds";

		[Serializable]
		public class PlayerData
		{
			[SerializeField]
			private string id;

			[SerializeField]
			private Sprite icon;

			[SerializeField]
			private string firstName;

			[SerializeField]
			private string lastName;

			public string Id { get { return id; } }
			public Sprite Icon { get { return icon; } }
			public string FirstName { get { return firstName; } }
			public string LastName { get { return lastName; } }
		}

		[SerializeField]
		private PlayerData[] players;

		private List<string> usedIds;


		private void Awake()
		{
			string save = PlayerPrefs.GetString (DATABASE_USED_IDS);

			if (string.IsNullOrEmpty (save))
				usedIds = new List<string> ();
			else
				usedIds = JsonUtility.FromJson<List<string>> (save);
		}

		private void OnDestroy()
		{
			string save = JsonUtility.ToJson (usedIds);
			PlayerPrefs.SetString (DATABASE_USED_IDS, save);
		}

		public PlayerData Get(string id)
		{
			return Array.FindLast (players, p => p.Id == id);
		}

		public PlayerData GetUnused()
		{
			var pd = Array.FindLast (players, p => !usedIds.Contains (p.Id));
			if (pd == null && usedIds.Count > 0)
			{
				usedIds.Clear ();
				return GetUnused ();
			}

			usedIds.Add (pd.Id);
			return pd;
		}
	}
}

