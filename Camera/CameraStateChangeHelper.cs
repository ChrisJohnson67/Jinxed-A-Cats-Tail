using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CameraStateChangeHelper
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[RequireComponent(typeof(BoxCollider2D))]
public class CameraStateChangeHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private CameraController.CameraState m_cameraState;

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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		LevelManager.Instance.CurrentLevel.CameraController.ChangeCameraState(m_cameraState);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{
		var collider = GetComponent<BoxCollider2D>();
		if (collider != null)
		{
			collider.isTrigger = true;
		}
	}
#endif
}