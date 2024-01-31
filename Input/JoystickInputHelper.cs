using UnityEngine;
using UnityEngine.Events;

public class JoystickInputHelper : RegisterInputHelper
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private UnityEvent<Vector2, bool> m_onLeftJoystick;

	[SerializeField]
	private UnityEvent<Vector2, bool> m_onRightJoystick;

	[SerializeField]
	private bool m_horizontal;

	[SerializeField]
	private bool m_waitForNeutral;

	[SerializeField]
	private float m_timedDelay;

	[SerializeField]
	private bool m_ignoreDeadspace;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	protected override void OnEnable()
	{
		base.OnEnable();
		InputListenerManager.Instance.EnableUIJoystickAdditionalDeadspace(InputObject);
	}

	protected override void Update()
	{
		base.Update();

		if (!m_listening)
			return;

		if (m_onLeftJoystick != null && m_onLeftJoystick.GetPersistentEventCount() > 0)
		{
			var dir = InputListenerManager.Instance.GetLeftJoystickDirection(InputObject, m_ignoreDeadspace);
			UpdateDirection(m_onLeftJoystick, dir, true);
		}

		if (m_onRightJoystick != null && m_onRightJoystick.GetPersistentEventCount() > 0)
		{
			var dir = InputListenerManager.Instance.GetRightJoystickDirection(InputObject, m_ignoreDeadspace);
			UpdateDirection(m_onRightJoystick, dir, false);
		}
	}

	protected void UpdateDirection(UnityEvent<Vector2, bool> a_callback, Vector2 a_dir, bool a_left)
	{
		if (m_horizontal)
		{
			if (a_dir.x < 0f)
			{
				SendDirectionUpdate(a_callback, a_dir, a_left);
			}
			else if (a_dir.x > 0f)
			{
				SendDirectionUpdate(a_callback, a_dir, a_left);
			}
		}
		else
		{
			if (a_dir.y < 0f)
			{
				SendDirectionUpdate(a_callback, a_dir, a_left);
			}
			else if (a_dir.y > 0f)
			{
				SendDirectionUpdate(a_callback, a_dir, a_left);
			}
		}
	}

	protected void SendDirectionUpdate(UnityEvent<Vector2, bool> a_callback, Vector2 a_dir, bool a_left)
	{
		a_callback?.Invoke(a_dir, m_horizontal);
		if (m_waitForNeutral)
		{
			if (a_left)
				InputListenerManager.Instance.WaitForLeftJoystickToNeutral(InputObject);
			else
				InputListenerManager.Instance.WaitForRightJoystickToNeutral(InputObject);
		}

		if (m_timedDelay > 0f)
		{
			InputListenerManager.Instance.SetJoystickDelayTimer(m_timedDelay);
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



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