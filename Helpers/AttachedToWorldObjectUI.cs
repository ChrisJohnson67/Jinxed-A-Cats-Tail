using Platform.UIManagement;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AttachedToCombatantUI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// Class is only abstract to prevent accidentally adding to a prefab. 
/// </summary>
[RequireComponent(typeof(UIPrefab)), DisallowMultipleComponent]
public abstract class AttachedToWorldObjectUI : MonoBehaviour
{
	#region Variables

	//--- Serialized ---

	[SerializeField]
	private bool m_stationary;

	//--- NonSerialized ---
	private Transform m_parentTransform;
	private RectTransform m_myRectTransform;

	#endregion Variables

	#region Accessors

	public Camera Camera { get; private set; }

	#endregion Accessors

	#region Unity Messages

	protected virtual void Update()
	{
		if (m_stationary)
		{
			return;
		}
		UpdatePosition();
	}

	#endregion Unity Messages

	#region Runtime Functions

	public virtual void Init(Transform a_worldTransform, Camera a_baseCamera)
	{
		Camera = a_baseCamera;
		m_parentTransform = a_worldTransform;
		m_myRectTransform = transform as RectTransform;

		UpdatePosition();
	}

	private void UpdatePosition()
	{
		if (Camera != null && m_myRectTransform != null)
		{
			m_myRectTransform.anchoredPosition = CameraHelpers.WorldToUISpace(Camera, GameManager.Instance.CanvasParent, m_parentTransform.position);
		}
	}

	#endregion Runtime Functions

	#region Editor Functions

#if UNITY_EDITOR



#endif

	#endregion Editor Functions
}