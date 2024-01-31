using UnityEngine;

[RequireComponent(typeof(InputWorldTrigger))]
public class DialogWorldTrigger : MonoBehaviour
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
	private InputWorldTrigger m_inputTrigger;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void OnEnable()
	{
		//check to see if the dialog can be repeated. If not, disable colliders
		var dialogTemplate = AssetCacher.Instance.CacheAsset<DialogTemplate>(m_dialogTID);
		if (dialogTemplate != null && m_inputTrigger != null)
		{
			if (!dialogTemplate.CanRepeat && SaveManager.Instance().WorldSaveData.HasDialogBeenTriggered(m_dialogTID))
			{
				m_inputTrigger.EnableColliders(false);
			}
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	public void OnDialogButtonPressed()
	{
		if (m_inputTrigger != null)
			m_inputTrigger.ShowInteractPopup(false);

		DialogManager.Instance.StartDialog(m_dialogTID, OnDialogFinished);
	}

	public void OnDialogFinished()
	{
		SaveManager.Instance().WorldSaveData.CompleteDialog(m_dialogTID);

		var dialogTemplate = AssetCacher.Instance.CacheAsset<DialogTemplate>(m_dialogTID);
		if (m_inputTrigger != null && dialogTemplate.CanRepeat)
			m_inputTrigger.ShowInteractPopup(true);
		else
			m_inputTrigger.RemoveInteraction();
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}