using Platform.UIManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : FullscreenUI
{
	#region Definitions

	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private float m_initialWaitTime = 2f;

	[SerializeField]
	private float m_speed;

	[SerializeField]
	private RectTransform m_scrollTransform;

	[SerializeField, TemplateIDField(typeof(Transform), "Scenes", "")]
	private List<int> m_levelScenes = new List<int>();

	[SerializeField, TemplateIDField(typeof(Transform), "Level Scene", "")]
	private int m_levelSceneTID;

	//--- NonSerialized ---
	private Transform m_levelScene;
	private bool m_scroll;
	private bool m_complete;
	private int m_currentSceneIndex = 0;

	#endregion Variables

	#region Accessors


	#endregion Accessors

	#region Unity Messages

	protected override void OnEnable()
	{
		base.OnEnable();
		m_levelScene = AssetCacher.Instance.InstantiateComponent<Transform>(m_levelSceneTID);
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds(m_initialWaitTime);

		m_scroll = true;
	}

	private void Update()
	{
		if (m_scroll)
			m_scrollTransform.localPosition = new Vector3(m_scrollTransform.localPosition.x, m_scrollTransform.localPosition.y + m_speed * Time.deltaTime, m_scrollTransform.localPosition.z);

		if (m_scrollTransform.anchoredPosition.y > 5342)
		{
			m_scroll = false;
			m_complete = true;
		}

		if (m_complete && InputListenerManager.Instance.WasGamepadButtonPressedThisFrame(UnityEngine.InputSystem.LowLevel.GamepadButton.A, gameObject))
		{
			m_complete = false;
			TransitionOverlay.FadeIn(5f, 4f, OnLoaded, null);
		}
	}

	#endregion Unity Messages

	#region Runtime Functions

	private void SwitchScene()
	{
		m_currentSceneIndex++;
		if (m_currentSceneIndex < m_levelScenes.Count)
		{
			var scene = AssetCacher.Instance.InstantiateComponent<Transform>(m_levelScenes[m_currentSceneIndex]);
		}
	}

	private void OnLoaded()
	{
		if (m_levelScene != null)
			Destroy(m_levelScene.gameObject);

		UIManager.Instance.CloseUI(this);
		UIManager.Instance.OpenUI<MainMenuUI>(GameManager.Instance.UISettings.TitleScreenTID);
	}


	#endregion Runtime Functions

	#region Callbacks


	#endregion Callbacks
}