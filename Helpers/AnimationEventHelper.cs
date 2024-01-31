using UnityEngine;
using UnityEngine.Events;

namespace FLIPClient
{
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	// AnimationEventHelper
	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public class AnimationEventHelper : MonoBehaviour
	{
		//~~~~~ Defintions ~~~~~
		#region Definitions


		#endregion Definitions

		//~~~~~ Variables ~~~~~
		#region Variables

		//--- Serialized ---
		[SerializeField]
		private UnityEvent m_onAnimEvent1;

		[SerializeField]
		private UnityEvent m_onAnimEvent2;

		[SerializeField]
		private UnityEvent m_onAnimEvent3;

		[SerializeField]
		private UnityEvent m_onAnimEvent4;

		//--- NonSerialized ---

		#endregion Variables

		//~~~~~ Accessors ~~~~~
		#region Accessors


		#endregion Accessors

		//~~~~~ Unity Messages ~~~~~
		#region Unity Messages

		#endregion Unity Messages

		//~~~~~ Runtime Functions ~~~~~
		#region Runtime Functions

		public void OnAnimationEvent1()
		{
			m_onAnimEvent1?.Invoke();
		}

		public void OnAnimationEvent2()
		{
			m_onAnimEvent2?.Invoke();
		}
		public void OnAnimationEvent3()
		{
			m_onAnimEvent3?.Invoke();
		}
		public void OnAnimationEvent4()
		{
			m_onAnimEvent4?.Invoke();
		}
		#endregion Runtime Functions

		//~~~~~ Callbacks ~~~~~
		#region Callbacks


		#endregion Callbacks

	}
}