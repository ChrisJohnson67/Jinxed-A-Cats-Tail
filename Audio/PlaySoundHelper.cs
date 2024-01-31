using FMODUnity;
using System.Collections;
using UnityEngine;

public class PlaySoundHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private EventReference m_sound;

	[SerializeField]
	private bool m_playOnEnable = true;

	[SerializeField]
	private float m_delay = 0f;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void OnEnable()
	{
		if (m_playOnEnable)
		{
			PlaySound();
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private IEnumerator DelayCR()
	{
		if (m_delay > 0f)
		{
			yield return new WaitForSeconds(m_delay);
		}

		ExecuteMusic();
	}

	public void PlaySound()
	{
		StartCoroutine(DelayCR());
	}

	private void ExecuteMusic()
	{
		SoundManager.Instance.PlaySound(m_sound);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}