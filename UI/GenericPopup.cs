using Platform.UIManagement;
using System;
using TMPro;
using UnityEngine;

public class GenericPopup : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private TMP_Text m_text;

	[SerializeField]
	private TMP_Text m_confirmButtonText;

	[SerializeField]
	private TMP_Text m_denyButtonText;

	[SerializeField]
	private GlowButton m_denyButton;

	//--- NonSerialized ---
	private Action m_onConfirm;
	private Action m_onDeny;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(string a_text, string a_confirmText, string a_denyText, Action a_onConfirm, Action a_onDeny = null)
	{
		m_onConfirm = a_onConfirm;
		m_onDeny = a_onDeny;

		if (m_text != null)
			m_text.text = a_text;

		if (m_confirmButtonText != null)
			m_confirmButtonText.text = a_confirmText;

		if (m_denyButtonText != null)
			m_denyButtonText.text = a_denyText;

		UIUtils.SetActive(m_denyButton, !string.IsNullOrEmpty(a_denyText));
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnConfirm()
	{
		m_onConfirm?.Invoke();
		UIManager.Instance.CloseUI(this);
	}

	public void OnDeny()
	{
		m_onDeny?.Invoke();
		UIManager.Instance.CloseUI(this);
	}

	#endregion Callbacks

	public static GenericPopup Open(string a_text, string a_confirmText, string a_denyText, Action a_onConfirm, Action a_onDeny = null)
	{
		var popup = UIManager.Instance.OpenUI<GenericPopup>(GameManager.Instance.UISettings.GenericPopupUITID);
		if (popup != null)
		{
			popup.Init(a_text, a_confirmText, a_denyText, a_onConfirm, a_onDeny);
		}
		return popup;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}