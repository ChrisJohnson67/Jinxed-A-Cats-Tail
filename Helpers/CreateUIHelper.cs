using Platform.UIManagement;
using UnityEngine;

public class CreateUIHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(UIPrefab), "UI", "")]
	private int m_uiTID;

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void CreateUI()
	{
		UIManager.Instance.OpenUI(m_uiTID);
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