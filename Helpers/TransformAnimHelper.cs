using System.Collections;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// TransformAnimHelper
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class TransformAnimHelper : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private float m_horizontalFloatRange;

	[SerializeField]
	private float m_verticalFloatRange;

	[SerializeField]
	private float m_verticalFloatTime;

	[SerializeField]
	private Vector3 m_rotationSpeed;

	[SerializeField]
	private bool m_physics;

	//--- NonSerialized ---
	private float m_timer;
	private bool m_floatingUp = true;
	private Vector3 m_startPos, m_endPos;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void OnEnable()
	{
		if (m_verticalFloatTime > 0f)
		{
			StartCoroutine(FloatCR());
		}
		if (m_rotationSpeed != Vector3.zero)
		{
			StartCoroutine(RotateCR());
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private IEnumerator FloatCR()
	{
		yield return null;

		while (true)
		{
			if (m_timer == 0f)
			{
				var posVec = transform.localPosition;
				m_startPos = posVec;
				m_endPos = new Vector2(posVec.x + ((m_floatingUp ? 1f : -1f) * m_horizontalFloatRange), posVec.y + ((m_floatingUp ? 1f : -1f) * m_verticalFloatRange));
			}

			while (m_timer < m_verticalFloatTime)
			{
				var newXPos = Mathf.SmoothStep(m_startPos.x, m_endPos.x, m_timer / m_verticalFloatTime);
				var newYPos = Mathf.SmoothStep(m_startPos.y, m_endPos.y, m_timer / m_verticalFloatTime);
				SetPosition(new Vector2(newXPos, newYPos));
				yield return null;
				m_timer += Time.deltaTime;
			}
			SetPosition(m_endPos);
			m_floatingUp = !m_floatingUp;
			m_timer = 0f;
		}
	}

	private void SetPosition(Vector2 a_newPos)
	{
		transform.localPosition = new Vector3(a_newPos.x, a_newPos.y, transform.localPosition.z);
	}

	private IEnumerator RotateCR()
	{
		var waitForFixedUpdate = new WaitForFixedUpdate();
		while (true)
		{
			transform.Rotate(m_rotationSpeed * (m_physics ? Time.fixedDeltaTime : Time.deltaTime));
			if (m_physics)
			{
				yield return waitForFixedUpdate;
			}
			else
			{
				yield return null;
			}
		}
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