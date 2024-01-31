using Platform.UIManagement;
using System;
using System.Collections;
using UnityEngine;

public class TransitionOverlay : MonoBehaviour
{
	[SerializeField]
	private CanvasGroup m_cg;

	private static TransitionOverlay sm_overlay;

	public static bool IsShowing { get { return sm_overlay != null; } }

	public static void FadeIn(float a_time, float a_holdTime, Action a_onFadeInComplete, Action a_fadeOutComplete)
	{
		if (sm_overlay != null)
		{
			a_onFadeInComplete?.Invoke();
			return;
		}
		if (sm_overlay == null)
		{
			sm_overlay = UIManager.Instance.OpenUI<TransitionOverlay>(GameManager.Instance.UISettings.TransitionOverlayTID);
		}

		GameManager.Instance.StartCoroutine(sm_overlay.FadeCR(a_time, a_holdTime, a_onFadeInComplete, a_fadeOutComplete));
	}

	public IEnumerator FadeCR(float a_time, float a_holdTime, Action a_fadeInComplete, Action a_fadeOutComplete)
	{
		float timer = 0f;
		while (timer < a_time)
		{
			var newFade = Mathf.Lerp(0f, 1f, timer / a_time);
			m_cg.alpha = newFade;
			yield return null;
			timer += Time.deltaTime;
		}
		m_cg.alpha = 1f;

		yield return new WaitForSeconds(a_holdTime);
		a_fadeInComplete?.Invoke();

		timer = 0f;
		while (timer < a_time)
		{
			var newFade = Mathf.Lerp(1f, 0f, timer / a_time);
			m_cg.alpha = newFade;
			yield return null;
			timer += Time.deltaTime;
		}
		m_cg.alpha = 0f;
		a_fadeOutComplete?.Invoke();

		CloseUI();
	}

	public void CloseUI()
	{
		UIManager.Instance.CloseUI(this);
		sm_overlay = null;
	}
}
