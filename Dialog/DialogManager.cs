using Platform.UIManagement;
using Platform.Utility;
using System;
using System.Collections;
using UnityEngine;

public class DialogManager : MonoSingleton<DialogManager>
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private float m_minDelayBetweenDialogSteps = 0.1f;

	//--- NonSerialized ---
	private DialogTemplate m_currentDialogTemplate;
	private int m_currentDialogStepIndex = 0;
	private DialogPanel m_currentDialogPanel;
	private Action m_onFinished;

	private float m_delayInputTimer;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void StartDialog(int a_dialogTemplateTID, Action a_onFinished)
	{
		m_onFinished = a_onFinished;
		m_currentDialogTemplate = AssetCacher.Instance.CacheAsset<DialogTemplate>(a_dialogTemplateTID);

		LevelManager.Instance.SetPaused(true);

		InputListenerManager.Instance.RegisterInputListener(gameObject);

		CreateDialogPanel();
	}

	private void CreateDialogPanel()
	{
		DestroyDialogPanel();
		if (m_currentDialogTemplate != null)
		{
			var dialogStep = m_currentDialogTemplate.GetStepForIndex(m_currentDialogStepIndex);
			int dialogTID = dialogStep.PlayerSpeaking ? GameManager.Instance.UISettings.DialogPanelPlayerTID : GameManager.Instance.UISettings.DialogPanelOtherTID;
			m_currentDialogPanel = UIManager.Instance.OpenUI<DialogPanel>(dialogTID);
			m_currentDialogPanel.Init(dialogStep);
		}
		else
		{
			EndDialogSequence();
		}
	}

	private void AdvanceToNextDialogStep()
	{
		SoundManager.Instance.PlaySound(GameManager.Instance.UISettings.UiClickSound);
		DestroyDialogPanel();
		m_currentDialogStepIndex++;
		if (m_currentDialogStepIndex >= m_currentDialogTemplate.StepCount)
		{
			EndDialogSequence();
			return;
		}

		m_delayInputTimer = m_minDelayBetweenDialogSteps;
		CreateDialogPanel();
	}

	private void EndDialogSequence()
	{
		LevelManager.Instance.SetPaused(false);
		StartCoroutine(WaitToDeregisterInputCR());
		m_currentDialogStepIndex = 0;
		m_currentDialogTemplate = null;
		m_onFinished?.Invoke();
	}

	private void Update()
	{
		if (m_currentDialogTemplate == null)
			return;

		if (CanAdvanceDialog())
		{
			if (!m_currentDialogPanel.IsTextFinishedTyping)
			{
				m_currentDialogPanel.FinishTextInstantly();
			}
			else
			{
				AdvanceToNextDialogStep();
			}
		}

		if (m_delayInputTimer > 0)
		{
			m_delayInputTimer -= Time.deltaTime;
		}
	}

	private bool CanAdvanceDialog()
	{
		return m_delayInputTimer <= 0 && InputListenerManager.Instance.WasInputButtonPressedThisFrame(GameManager.Instance.InputSettings.DialogAdvanceInput, gameObject);
	}

	private void DestroyDialogPanel()
	{
		if (m_currentDialogPanel != null)
		{
			GameManager.Instance.DeleteObject(m_currentDialogPanel);
			m_currentDialogPanel = null;
		}
	}

	private IEnumerator WaitToDeregisterInputCR()
	{
		yield return null;

		InputListenerManager.Instance.DeregisterInputListener(gameObject);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks
}
