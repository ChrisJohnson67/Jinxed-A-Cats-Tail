using UnityEngine;

public class AnimatorPauseHelper : MonoBehaviour
{
	#region Definitions


	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Animator m_animator;

	//--- NonSerialized ---

	#endregion Variables

	#region Accessors


	#endregion Accessors

	#region Unity Messages

	void OnEnable()
	{
		LevelManager.OnPaused += OnPaused;
	}

	void OnDisable()
	{
		LevelManager.OnPaused -= OnPaused;
	}

	#endregion Unity Messages

	#region Runtime Functions

	#endregion Runtime Functions

	#region Callbacks

	private void OnPaused(bool a_paused)
	{
		if (m_animator != null)
			m_animator.speed = a_paused ? 0f : 1f;
	}

	#endregion Callbacks

	private void OnValidate()
	{
		if (m_animator == null)
			m_animator = GetComponentInChildren<Animator>();
	}
}

