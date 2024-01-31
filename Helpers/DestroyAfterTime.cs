using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	[SerializeField]
	private float m_timeToDestroy = 1f;

	[SerializeField]
	private bool m_shrink = true;

	private float m_timer = 0f;

	private void Update()
	{
		m_timer += Time.deltaTime;
		if (m_timer >= m_timeToDestroy)
		{
			if (m_shrink)
			{
				StartCoroutine(Shrink());
			}
			else
			{
				GameManager.Instance.DeleteObject(gameObject);
			}
		}
	}

	private IEnumerator Shrink()
	{
		float timer = 0f;
		var startScale = transform.localScale;
		while (timer < 1f)
		{
			transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer);
			yield return null;
			timer += Time.deltaTime;
		}

		GameManager.Instance.DeleteObject(gameObject);
	}
}
