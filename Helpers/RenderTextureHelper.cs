using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// RenderTextureHelper
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class RenderTextureHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_parent;

	[SerializeField]
	private Camera m_camera;

	//--- NonSerialized ---
	private GameObject m_object;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(GameObject a_obj)
	{
		m_object = a_obj;
		if (m_object == null)
		{
			Destroy(m_object);
		}
		if (m_object != null)
		{
			m_object.transform.SetParent(m_parent, false);
		}
	}

	public void SetRenderTexture(RenderTexture a_tex)
	{
		if (m_camera != null)
			m_camera.targetTexture = a_tex;
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