using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

using Utils;
using Players;


namespace Controllers
{
    public class BoardController : SingletonMonoBehaviour<BoardController>
    {
		[Serializable]
		private class PlayerSlotData
		{
			[SerializeField]
			private Transform slotTransform;

			[SerializeField]
			private bool isUserAccessable;

			public Transform SlotTransform { get { return slotTransform; } }
			public bool IsUserAccessable { get { return isUserAccessable; } }

			public bool IsBusy { get; set; }
		}

        [SerializeField]
		private PlayerSlotData[] slotsData;
        
        private List<Player> playerInstances;


		protected override void Awake ()
		{
			base.Awake ();

			playerInstances = new List<Player> ();
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			foreach (var player in playerInstances)
				Pool.Instance.Retrieve (player.gameObject);

			playerInstances.Clear ();
		}

        public int GetPlayerCount ()
        {
			return playerInstances.Count;
        }

        public Player GetPlayer(int index)
        {
			return playerInstances[index];
        }

		public void AddPlayer(Player player)
		{
			Transform slot = GetFreeSlot (UserController.Instance.GetPlayer () == player);

			player.transform.SetParent (slot);
			player.transform.localPosition = Vector3.zero;
			player.transform.localRotation = Quaternion.identity;
			player.gameObject.SetActive (true);

			playerInstances.Add (player);
		}

		public IEnumerable<Player> GetPlayers()
        {
			return playerInstances;
        }

		private Transform GetFreeSlot(bool isUser)
		{
			//var slots = Array.FindAll (slotsData, sd => !sd.IsBusy && sd.IsUserAccessable == isUser);
            var slots = Array.FindAll(slotsData, sd => !sd.IsBusy);
            if (slots != null && slots.Length > 0)
			{
				PlayerSlotData slot = slots[0];
				slot.IsBusy = true;

				return slot.SlotTransform;
			}

			return this.transform;
		}

		public void ClearBoard()
		{
			foreach (var player in playerInstances)
			{
				Pool.Instance.Retrieve (player.gameObject);
			}

			playerInstances.Clear ();

			foreach (var slot in slotsData)
				slot.IsBusy = false;
		}

		public static Player CreatePlayer()
		{
			return Pool.Instance.Get (CommonPrototypesController.Instance.Player).GetComponent<Player> ();
		}

		public static Sharper CreateSharper()
		{
			return Pool.Instance.Get (CommonPrototypesController.Instance.Sharper).GetComponent<Sharper> ();
		}

		private void OnDrawGizmos()
		{
			if (slotsData != null)
			{
				foreach (var slot in slotsData)
				{
					if (slot.SlotTransform != null)
					{
						Gizmos.DrawSphere (slot.SlotTransform.position, 1.0f);
						Gizmos.DrawRay (slot.SlotTransform.position, slot.SlotTransform.forward * 5);
					}
				}
			}
		}
    }
}
