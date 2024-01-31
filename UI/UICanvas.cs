using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private CanvasScaler m_canvasScaler;
	[SerializeField]
	private Canvas m_canvas;
	[SerializeField]
	private RectTransform m_canvasRect;

	//--- NonSerialized ---
	private float m_actualScreenRatio;
	private float m_refScreenRatio;
	private float m_scaleFactor;

	private static Dictionary<string, UICanvas> sm_tagToCanvasDict = new Dictionary<string, UICanvas>();

	#endregion Variables

	#region Attributes

	public Canvas Canvas => m_canvas;
	public RectTransform CanvasRect => m_canvasRect;

	#endregion

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	protected void OnEnable()
	{
		RegisterUICanvas(gameObject.tag, this);
		if (m_canvasScaler == null)
		{
			return;
		}
		m_refScreenRatio = m_canvasScaler.referenceResolution.x / m_canvasScaler.referenceResolution.y;
		UpdateScreenRatios();
	}

	protected void OnDisable()
	{
		UnregisterUICanvas(gameObject.tag);
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	private static void RegisterUICanvas(string a_tag, UICanvas a_canvas)
	{
		if (string.IsNullOrEmpty(a_tag) || a_canvas == null)
		{
			return;
		}
		sm_tagToCanvasDict[a_tag] = a_canvas;
	}

	private static void UnregisterUICanvas(string a_tag)
	{
		if (string.IsNullOrEmpty(a_tag))
		{
			return;
		}
		if (!sm_tagToCanvasDict.ContainsKey(a_tag))
		{
			return;
		}
		sm_tagToCanvasDict.Remove(a_tag);
	}

	public void UpdateScreenRatios()
	{
		if (m_canvasScaler == null)
		{
			return;
		}

		//Truncate to two decimal places -- keeps "near ratios" from hitting
		m_actualScreenRatio = (int)(((float)Screen.width / Screen.height) * 100);
		m_actualScreenRatio /= 100f;

		if (m_actualScreenRatio > m_refScreenRatio)
		{
			m_canvasScaler.matchWidthOrHeight = 1f;
			m_scaleFactor = (m_canvasScaler.referenceResolution.y / Screen.height);
		}
		else
		{
			m_canvasScaler.matchWidthOrHeight = 0f;
			m_scaleFactor = m_canvasScaler.referenceResolution.x / Screen.width;
		}
	}

	public Vector3 ViewportToCanvas(Vector3 a_viewportPos)
	{
		if (m_canvasRect == null)
		{
			return Vector3.zero;
		}

		Vector2 hudPos = new Vector2((a_viewportPos.x - m_canvasRect.pivot.x) * m_canvasRect.rect.width, (a_viewportPos.y - m_canvasRect.pivot.y) * m_canvasRect.rect.height);
		return hudPos;
	}

	public Vector3 ViewportToScreenDelta(Vector3 a_startViewportPos, Vector3 a_endViewportPos)
	{
		return ViewportToCanvas(a_endViewportPos) - ViewportToCanvas(a_startViewportPos);
	}

	public Vector3 CorrectedScreen2DPoint(Vector2 a_pos, bool a_update = false)
	{
		if (a_update)
		{
			UpdateScreenRatios();
		}
		return new Vector3(a_pos.x * m_scaleFactor, a_pos.y * m_scaleFactor, 0f);
	}

	public static UICanvas GetUICanvas(string a_tag)
	{
		if (string.IsNullOrEmpty(a_tag))
		{
			return null;
		}
		UICanvas canvas = null;
		sm_tagToCanvasDict.TryGetValue(a_tag, out canvas);
		return canvas;
	}

	#endregion Runtime Functions

	//~~~~~ Editor Functions ~~~~~
	#region Editor Functions

#if UNITY_EDITOR

	public void Reset()
	{
		OnValidate();
	}

	public void OnValidate()
	{
		if (m_canvas == null)
		{
			m_canvas = gameObject.GetComponentInParent<Canvas>();
		}
		if (m_canvasRect == null)
		{
			m_canvasRect = gameObject.GetComponent<RectTransform>();
		}
		if (m_canvasScaler == null)
		{
			m_canvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
		}
	}

#endif

	#endregion Editor Functions
}
