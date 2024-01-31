using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BOW
{
	public class TriggerEventAfterTime : MonoBehaviour
	{
		#region Definitions


		#endregion Definitions

		#region Variables

		//--- Serialized ---
		[SerializeField]
		private float m_duration;

		[SerializeField]
		private UnityEvent m_event;

		//--- NonSerialized ---

		#endregion Variables

		#region Accessors


		#endregion Accessors

		#region Unity Messages

		private void OnEnable()
		{
			StartCoroutine(WaitToDoEventCR());
		}

		#endregion Unity Messages

		#region Runtime Functions

		private IEnumerator WaitToDoEventCR()
		{
			yield return new WaitForSeconds(m_duration);

			if (this != null)
				m_event?.Invoke();
		}

		#endregion Runtime Functions

		#region Callbacks


		#endregion Callbacks
	}
}