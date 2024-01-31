using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[Serializable]
public class InputConfig
{
	[SerializeField]
	private GamepadButton m_gamepadButton;

	[SerializeField]
	private Key m_keyboardKey;

	public GamepadButton Button => m_gamepadButton;
	public Key Key => m_keyboardKey;
}