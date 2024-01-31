using UnityEngine;

[RequireComponent(typeof(TemplateID))]
public class FXObject : MonoBehaviour
{
	[SerializeField]
	private float m_liveTime;

	[SerializeField]
	private ParticleSystem m_particleSystem;

	private Transform m_followTarget;
	private float m_timer = 0f;

	private void Update()
	{
		if (m_followTarget != null)
		{
			transform.position = m_followTarget.position;
		}

		if (m_liveTime > 0f)
		{
			m_timer += Time.deltaTime;

			if (m_timer >= m_liveTime)
			{
				GameManager.Instance.DeleteObject(gameObject);
			}
		}
	}

	public void SetFollowTarget(Transform a_target)
	{
		m_followTarget = a_target;
	}

	public void OnPaused(bool a_paused)
	{
		if (m_particleSystem == null)
			return;

		if (a_paused)
		{
			m_particleSystem.Pause();
		}
		else
		{
			m_particleSystem.Play();
		}
	}

	public static FXObject CreateAtPosition(int a_fxTID, Transform a_parent)
	{
		return CreateAtPosition(a_fxTID, a_parent.position);
	}

	public static FXObject CreateUnderParent(int a_fxTID, Transform a_parent)
	{
		return AssetCacher.Instance.InstantiateComponent<FXObject>(a_fxTID, a_parent);
	}

	public static FXObject CreateAtPosition(int a_fxTID, Vector3 a_position)
	{
		var obj = AssetCacher.Instance.InstantiateComponent<FXObject>(a_fxTID);
		if (obj != null)
		{
			obj.transform.position = a_position;
		}
		return obj;
	}

	public static FXObject Create(GameObject a_fx, Transform a_parent)
	{
		var obj = Instantiate(a_fx, a_parent);
		return obj.GetComponent<FXObject>();
	}
}
