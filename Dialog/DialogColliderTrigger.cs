using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ColliderHelper2D))]
public class DialogColliderTrigger : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(DialogTemplate), "Dialog Template", "")]
	private int m_dialogTID;

	[SerializeField]
	private Collider2D m_collider;

	[SerializeField]
	private UnityEvent m_onDialogComplete;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnStartDialog()
	{
		SoundManager.Instance.PlaySound(GameManager.Instance.UISettings.UiClickSound);
		DialogManager.Instance.StartDialog(m_dialogTID, OnDialogFinished);
	}

	private void OnDialogFinished()
	{
		SaveManager.Instance().WorldSaveData.CompleteDialog(m_dialogTID);
		if (m_collider != null)
		{
			UIUtils.SetActive(m_collider, false);
		}
		m_onDialogComplete?.Invoke();
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (m_collider == null)
			m_collider = GetComponentInChildren<Collider2D>();
	}
#endif
}