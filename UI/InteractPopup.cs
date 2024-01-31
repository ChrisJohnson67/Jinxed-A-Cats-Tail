using System;
using TMPro;
using UnityEngine;

public class InteractPopup : AttachedToWorldObjectUI
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_interactText;

	//--- NonSerialized ---
	private InputConfig m_input;
	private Action m_onPressed;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(Transform a_parent, InputConfig a_input, Action a_onPressed)
	{
		base.Init(a_parent, LevelManager.Instance.CurrentLevel.Camera);
		m_input = a_input;
		m_onPressed = a_onPressed;
	}

	protected override void Update()
	{
		base.Update();
		if (InputListenerManager.Instance.WasInputButtonPressedThisFrame(m_input, LevelManager.Instance.CurrentLevel.PlayerUnit.PlatformController.gameObject))
		{
			m_onPressed?.Invoke();
		}
	}
	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}