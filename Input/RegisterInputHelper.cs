using UnityEngine;
using UnityEngine.Events;

public class RegisterInputHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private RegisterInputHelper m_parentInputHelper;

	[SerializeField]
	private UnityEvent m_onBack;

	//--- NonSerialized ---
	private GameObject m_inputObject;
	protected bool m_listening;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public GameObject InputObject => m_inputObject;

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		m_inputObject = gameObject;
		if (m_parentInputHelper != null)
		{
			m_inputObject = m_parentInputHelper.gameObject;
		}
	}

	protected virtual void OnEnable()
	{
		InputListenerManager.Instance.RegisterInputListener(m_inputObject);
		m_listening = true;
	}

	protected virtual void OnDisable()
	{
		if (InputListenerManager.HasInstance)
			InputListenerManager.Instance.DeregisterInputListener(m_inputObject);

		m_listening = false;
	}

	protected virtual void Update()
	{
		if (!m_listening)
			return;

		if (m_onBack != null && m_onBack.GetPersistentEventCount() > 0)
		{
			if (InputListenerManager.Instance.WasInputButtonPressedThisFrame(GameManager.Instance.InputSettings.BackInput, gameObject))
			{
				m_onBack?.Invoke();
			}
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void SetListening(bool a_listening)
	{
		m_listening = a_listening;
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