using Platform.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// InputListenerManager
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class InputListenerManager : MonoSingleton<InputListenerManager>
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public class InputObject
	{
		public int ObjectID { get; set; }
		public bool WaitingForLeftJoystickNeutral { get; set; }
		public bool WaitingForRightJoystickNeutral { get; set; }
		public bool UIJoystickDeadspaceEnabled { get; set; }

	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Vector2 m_joystickDeadSpace;

	//--- NonSerialized ---
	private List<InputObject> m_objectListenerList = new List<InputObject>();
	private InputObject m_currentInputListener;
	private Keyboard m_keyboardInput;
	private Mouse m_mouseInput;
	private Gamepad m_gamepadInput;
	private bool m_inputEnabled = true;
	private float m_joystickDelayTimer;


	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public Keyboard Keyboard
	{
		get
		{
			if (m_keyboardInput == null)
			{
				SetKeyboard();
			}
			return m_keyboardInput;
		}
	}

	public Mouse Mouse
	{
		get
		{
			if (m_mouseInput == null)
			{
				SetMouse();
			}
			return m_mouseInput;
		}
	}

	public Gamepad Gamepad
	{
		get
		{
			if (m_gamepadInput == null)
			{
				SetGamepad();
			}
			return m_gamepadInput;
		}
	}

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	protected override void Awake()
	{
		base.Awake();
		SetKeyboard();
		SetMouse();
		SetGamepad();
	}

	private void Update()
	{
		m_joystickDelayTimer -= Time.deltaTime;
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void SetInputEnabled(bool a_enabled)
	{
		m_inputEnabled = a_enabled;
	}

	public void RegisterInputListener(GameObject a_obj)
	{
		RegisterInputListener(a_obj.GetInstanceID());
	}

	public void RegisterInputListener(int a_id)
	{
		var inputObject = m_objectListenerList.Find(x => x.ObjectID == a_id);
		if (inputObject == null)
		{
			inputObject = new InputObject()
			{
				ObjectID = a_id
			};

			m_objectListenerList.Add(inputObject);
		}
		m_currentInputListener = inputObject;
		m_joystickDelayTimer = 0f;
	}

	public void DeregisterInputListener(GameObject a_obj)
	{
		DeregisterInputListener(a_obj.GetInstanceID());
	}

	public void DeregisterInputListener(int a_id)
	{
		m_objectListenerList.RemoveAll(x => x.ObjectID == a_id);

		if (m_objectListenerList.Count > 0)
		{
			m_currentInputListener = m_objectListenerList[m_objectListenerList.Count - 1];
		}
		else
		{
			m_currentInputListener = null;
		}
		m_joystickDelayTimer = 0f;
	}

	public bool CanListenForInput(GameObject a_gameObject)
	{
		if (!m_inputEnabled)
			return false;

		//if there is no primary listener, then input is valid

		if (m_currentInputListener == null || a_gameObject == null)
		{
			return true;
		}

		//if there is a primary listener, make sure it matches the gameobject trying to use the input
		int gid = a_gameObject.GetInstanceID();
		if (gid == m_currentInputListener.ObjectID)
		{
			return true;
		}

		return false;
	}

	private InputObject GetInputObject(GameObject a_obj)
	{
		return m_objectListenerList.Find(x => x.ObjectID == a_obj.GetInstanceID());
	}

	public void WaitForLeftJoystickToNeutral(GameObject a_obj)
	{
		var inputObject = GetInputObject(a_obj);
		if (inputObject != null)
		{
			inputObject.WaitingForLeftJoystickNeutral = true;
		}
	}

	public void WaitForRightJoystickToNeutral(GameObject a_obj)
	{
		var inputObject = GetInputObject(a_obj);
		if (inputObject != null)
		{
			inputObject.WaitingForRightJoystickNeutral = true;
		}
	}

	public void SetJoystickDelayTimer(float a_time)
	{
		m_joystickDelayTimer = a_time;
	}

	public void EnableUIJoystickAdditionalDeadspace(GameObject a_obj)
	{
		var inputObject = GetInputObject(a_obj);
		if (inputObject != null)
		{
			inputObject.UIJoystickDeadspaceEnabled = true;
		}
	}

	public bool IsKeyHeldDown(Key a_key, GameObject a_object)
	{
		if (!CanListenForInput(a_object))
			return false;

		if (Keyboard != null)
		{
			return m_keyboardInput[a_key].isPressed;
		}
		return false;
	}

	public bool WasKeyPressedThisFrame(Key a_key, GameObject a_object)
	{
		if (!CanListenForInput(a_object))
			return false;

		if (Keyboard != null)
		{
			return m_keyboardInput[a_key].wasPressedThisFrame;
		}
		return false;
	}

	public bool WasKeyReleasedThisFrame(Key a_key, GameObject a_object)
	{
		if (!CanListenForInput(a_object))
			return false;

		if (Keyboard != null)
		{
			return m_keyboardInput[a_key].wasReleasedThisFrame;
		}
		return false;
	}

	public bool WasAnyKeyPressedThisFrame(GameObject a_object)
	{
		if (!CanListenForInput(a_object))
			return false;

		if (Keyboard != null)
		{
			return m_keyboardInput.anyKey.wasPressedThisFrame;
		}
		return false;
	}

	public bool WasMouseButtonPressed(bool a_left, GameObject a_object)
	{
		if (!CanListenForInput(a_object))
			return false;

		if (Mouse != null)
		{
			return a_left ? m_mouseInput.leftButton.wasPressedThisFrame : m_mouseInput.rightButton.wasPressedThisFrame;
		}
		return false;
	}

	#region Gamepad Controls

	public bool IsGamepadButtonHeldDown(GamepadButton a_key, GameObject a_object)
	{
		if (!CanListenForInput(a_object))
			return false;

		if (Gamepad != null)
		{
			return m_gamepadInput[a_key].isPressed;
		}
		return false;
	}

	public bool WasGamepadButtonPressedThisFrame(GamepadButton a_key, GameObject a_object)
	{
		if (!CanListenForInput(a_object))
			return false;

		if (Gamepad != null)
		{
			return m_gamepadInput[a_key].wasPressedThisFrame;
		}
		return false;
	}

	public bool WasGamepadButtonReleasedThisFrame(GamepadButton a_key, GameObject a_object)
	{
		if (!CanListenForInput(a_object))
			return false;

		if (Gamepad != null)
		{
			return m_gamepadInput[a_key].wasReleasedThisFrame;
		}
		return false;
	}

	public Vector2 GetLeftJoystickDirection(GameObject a_object, bool a_ignoreDeadspace = true)
	{
		if (!CanListenForInput(a_object))
			return Vector2.zero;

		if (Gamepad == null)
		{
			return Vector2.zero;
		}

		var inputObject = GetInputObject(a_object);
		var dir = GetJoystickDirection(m_gamepadInput.leftStick, a_object, a_ignoreDeadspace);
		if (inputObject.WaitingForLeftJoystickNeutral)
		{
			if (dir.x == 0f && dir.y == 0f)
			{
				inputObject.WaitingForLeftJoystickNeutral = false;
			}
			else
			{
				dir = Vector2.zero;
			}
		}
		return dir;
	}

	public Vector2 GetRightJoystickDirection(GameObject a_object, bool a_ignoreDeadspace = true)
	{
		if (!CanListenForInput(a_object))
			return Vector2.zero;

		if (Gamepad == null)
		{
			return Vector2.zero;
		}

		var inputObject = GetInputObject(a_object);
		var dir = GetJoystickDirection(m_gamepadInput.rightStick, a_object, a_ignoreDeadspace);
		if (inputObject.WaitingForRightJoystickNeutral)
		{
			if (dir.x == 0f && dir.y == 0f)
			{
				inputObject.WaitingForRightJoystickNeutral = false;
			}
			else
			{
				dir = Vector2.zero;
			}
		}
		return dir;
	}

	private Vector2 GetJoystickDirection(StickControl a_stick, GameObject a_object, bool a_ignoreDeadspace)
	{
		if (m_joystickDelayTimer > 0)
			return Vector2.zero;

		if (Gamepad != null)
		{
			var val = a_stick.ReadValue();
			if (a_ignoreDeadspace)
			{
				return val;
			}
			else
			{
				var inputObject = GetInputObject(a_object);
				float additionalDeadspace = inputObject.UIJoystickDeadspaceEnabled ? GameManager.Instance.InputSettings.UIAdditionalJoystickDeadspace : 0f;
				float x = val.x;
				float y = val.y;
				if (Mathf.Abs(x) > m_joystickDeadSpace.x + additionalDeadspace ||
					Mathf.Abs(y) > m_joystickDeadSpace.y + additionalDeadspace)
				{
					return val;
				}
				else
				{
					return Vector2.zero;
				}
			}
		}
		return Vector2.zero;
	}

	#endregion Gamepad Controls

	public bool WasInputButtonPressedThisFrame(InputConfig a_input, GameObject a_object)
	{
		return WasGamepadButtonPressedThisFrame(a_input.Button, a_object) || WasKeyPressedThisFrame(a_input.Key, a_object);
	}

	public Vector2 GetMousePosition()
	{
		if (Mouse != null)
		{
			return m_mouseInput.position.ReadValue();
		}
		return Vector2.zero;
	}

	private void SetKeyboard()
	{
		m_keyboardInput = InputSystem.GetDevice<Keyboard>();
	}

	private void SetMouse()
	{
		m_mouseInput = InputSystem.GetDevice<Mouse>();
	}

	private void SetGamepad()
	{
		m_gamepadInput = InputSystem.GetDevice<Gamepad>();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}