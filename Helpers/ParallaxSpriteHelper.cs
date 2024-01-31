using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxSpriteHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private SpriteRenderer m_spriteRenderer;

	//--- NonSerialized ---
	private Vector3 m_startPos;
	private Camera m_camera;
	private float m_parallaxValue;
	private Vector3 m_startTravelDistance;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void SetParallaxFollowTransform(Camera a_camera, float a_parallaxValue)
	{
		m_camera = a_camera;
		m_parallaxValue = a_parallaxValue;

		m_startPos = transform.position;
		m_startTravelDistance = (m_camera.transform.position - m_startPos + new Vector3(1f, 0f, 0f)) * m_parallaxValue;
		UpdateParallaxObject();
	}

	private void Update()
	{
		UpdateParallaxObject();
	}

	protected void UpdateParallaxObject()
	{
		if (m_camera != null)
		{
			var travelDist = m_camera.transform.position - m_startPos - m_startTravelDistance;
			travelDist.Set(travelDist.x, 0f, travelDist.z);
			transform.position = m_startPos + travelDist * m_parallaxValue;
		}
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (m_spriteRenderer == null)
		{
			m_spriteRenderer = GetComponent<SpriteRenderer>();
		}
	}
#endif
}
