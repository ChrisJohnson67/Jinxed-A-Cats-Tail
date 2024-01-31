using System;
using System.Collections;
using UnityEngine;

public class CountdownHelper
{
	public static void StartCountdown(float a_seconds, Action a_countdownComplete)
	{
		GameManager.Instance.StartCoroutine(CountdownCR(a_seconds, a_countdownComplete));
	}

	private static IEnumerator CountdownCR(float a_seconds, Action a_complete)
	{
		float timer = 0f;
		while (timer < a_seconds)
		{
			yield return null;
			timer += Time.deltaTime;
		}
		a_complete?.Invoke();
	}
}
