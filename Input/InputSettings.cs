using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InputSettings")]
public class InputSettings : TemplateObject
{
	//~~~~~ Definitions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField]
	private InputConfig m_interactInput;

	[SerializeField]
	private InputConfig m_dialogAdvanceInput;

	[SerializeField]
	private InputConfig m_confirmButtonForMenu;

	[SerializeField]
	private InputConfig m_backInput;



	[SerializeField]
	private float m_uiAdditionalJoystickDeadspace = 0.5f;
	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public InputConfig InteractInput { get { return m_interactInput; } }
	public InputConfig DialogAdvanceInput { get { return m_dialogAdvanceInput; } }
	public InputConfig ConfirmButtonForMenu { get { return m_confirmButtonForMenu; } }
	public InputConfig BackInput { get { return m_backInput; } }

	public float UIAdditionalJoystickDeadspace => m_uiAdditionalJoystickDeadspace;

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	#endregion Runtime Functions
}
