using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DialogTemplate")]
public class DialogTemplate : TemplateObject
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	[Serializable]
	public class DialogStep
	{
		[SerializeField, TemplateIDField(typeof(DisplayTemplate), "DisplayTemplate", "")]
		private int m_displayTemplateTID;

		[SerializeField, TextArea]
		private string m_text;

		[SerializeField]
		private bool m_playerSpeaking;

		[SerializeField]
		private bool m_playTypingAnim = true;

		public int DisplayTemplateTID { get => m_displayTemplateTID; }
		public string Text { get { return m_text; } }
		public bool PlayerSpeaking { get { return m_playerSpeaking; } }
		public bool PlayTypingAnim => m_playTypingAnim;
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private List<DialogStep> m_steps = new List<DialogStep>();

	[SerializeField]
	private bool m_canRepeat = true;

	[SerializeField]
	private bool m_persistInSaveData;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int StepCount { get { return m_steps.Count; } }
	public bool CanRepeat { get { return m_canRepeat; } }
	public bool PersistInSaveData => m_persistInSaveData;

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public DialogStep GetStepForIndex(int a_index)
	{
		if (a_index < 0 || a_index >= m_steps.Count)
			return null;

		return m_steps[a_index];
	}

	#endregion Runtime Functions

}
