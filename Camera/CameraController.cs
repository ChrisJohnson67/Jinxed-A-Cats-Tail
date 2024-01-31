using System;
using System.Collections;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CameraController
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class CameraController : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	public enum CameraState
	{
		LockedHeight,
		FollowCharacterHeight
	}

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private Transform m_movementTransform;

	[SerializeField]
	private Camera m_camera;

	[SerializeField]
	private float m_moveUpdateTime;

	[SerializeField]
	private float m_moveTime;

	[SerializeField]
	private float m_newHeightMoveTime;

	[SerializeField]
	private float m_zoomTime;

	[SerializeField]
	private float m_zoomSpeed;

	[SerializeField]
	private Transform m_frontOfPlayerFXParent;

	[SerializeField]
	private Transform m_abovePlayerFXParent;

	//--- NonSerialized ---
	private PlayerUnitInstance m_followUnit;
	private Transform m_followTransform;
	private Coroutine m_moveCR;
	private Coroutine m_zoomCR;
	private Vector3 m_moveVelocity;
	private Vector3 m_moveEndPos;
	private float m_moveUpdateTimer;
	private float m_lowerBound;
	private float m_upperBound;
	private CameraState m_cameraState = CameraState.LockedHeight;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public Camera Camera { get { return m_camera; } }
	public Transform MoveTransform => m_movementTransform;

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	private void OnEnable()
	{
	}

	#region Unity Messages
	private void OnDisable()
	{
		StopAllCoroutines();
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Init(Level a_level, PlayerUnitInstance a_unit)
	{
		m_followUnit = a_unit;
		m_followTransform = m_followUnit.PlayerModel.CameraFollowTransform;

		m_cameraState = a_level.ShouldFollowPlayerUpHigh ? CameraState.FollowCharacterHeight : CameraState.LockedHeight;

		var orthoSize = m_camera.orthographicSize;
		m_lowerBound = LevelManager.Instance.CurrentLevel.LowerCameraBound + orthoSize;
		m_upperBound = LevelManager.Instance.CurrentLevel.UpperCameraBound - orthoSize;

		SetHeight(m_followUnit.PlayerModel.CameraFollowTransform.position.y);
		SetHorzPosition(m_followTransform.position.x);
	}

	private void Update()
	{
		if (m_followTransform == null)
			return;

		m_moveUpdateTimer += Time.deltaTime;
		if (m_moveUpdateTimer >= m_moveUpdateTime)
		{
			m_moveUpdateTimer -= m_moveTime;
		}
		m_moveEndPos = m_followTransform.position;

		var newHorzPosition = Vector3.SmoothDamp(m_movementTransform.position, m_moveEndPos, ref m_moveVelocity, m_moveTime);
		SetHorzPosition(newHorzPosition.x);
		if (m_cameraState == CameraState.FollowCharacterHeight)
		{
			var newVertPosition = Vector3.SmoothDamp(m_movementTransform.position, m_moveEndPos, ref m_moveVelocity, m_newHeightMoveTime);
			SetHeight(newVertPosition.y);
		}
	}

	private void SetHeight(float a_newHeight)
	{
		var transPos = m_movementTransform.position;

		float newHeight = Mathf.Max(m_lowerBound, a_newHeight);
		if (LevelManager.Instance.CurrentLevel.LimitCameraToUpperBound)
		{
			newHeight = Mathf.Min(m_upperBound, newHeight);
		}

		m_movementTransform.position = new Vector3(transPos.x, newHeight, transPos.z);
	}

	private void SetHorzPosition(float a_newPos)
	{
		var currentLevel = LevelManager.Instance.CurrentLevel;

		var aspect = (float)Screen.width / Screen.height;
		var worldHeight = m_camera.orthographicSize * 2;
		var worldWidth = worldHeight * aspect;

		var newXPos = Mathf.Max(currentLevel.LeftCameraBound + (worldWidth * 0.5f), Mathf.Min(a_newPos, (currentLevel.RightCameraBound - worldWidth * 0.5f)));
		var transPos = m_movementTransform.position;
		m_movementTransform.position = new Vector3(newXPos, transPos.y, transPos.z);
	}

	public void TranslateCamera(Vector3 a_position, Action a_onComplete)
	{
		MoveTo(m_movementTransform.position + a_position, a_onComplete);
	}

	public void MoveTo(Vector3 a_position, Action a_onComplete)
	{
		if (m_moveCR != null)
		{
			StopCoroutine(m_moveCR);
		}
		m_moveCR = StartCoroutine(MoveCR(a_position, a_onComplete));
	}

	private IEnumerator MoveCR(Vector3 a_position, Action a_onComplete)
	{
		var timer = 0f;
		var startPosition = m_movementTransform.position;
		var endPosition = a_position;
		while (timer < m_moveTime)
		{
			var time = timer / m_moveTime;
			float newX = Mathf.SmoothStep(startPosition.x, endPosition.x, time);
			float newY = Mathf.SmoothStep(startPosition.y, endPosition.y, time);
			float newZ = Mathf.SmoothStep(startPosition.z, endPosition.z, time);
			m_movementTransform.position = new Vector3(newX, newY, newZ);
			yield return null;
			timer += Time.deltaTime;
		}
		m_movementTransform.position = endPosition;
		a_onComplete?.Invoke();
		m_moveCR = null;
	}

	public void ChangeCameraState(CameraState a_state)
	{
		m_cameraState = a_state;
	}

	public void ZoomOut(Action a_onComplete)
	{
		if (m_zoomCR != null)
		{
			StopCoroutine(m_zoomCR);
		}
		m_zoomCR = StartCoroutine(ZoomOutCR(a_onComplete));
	}


	public IEnumerator ZoomOutCR(Action a_onComplete)
	{
		var timer = 0f;
		var direction = m_camera.transform.forward;
		while (timer < m_zoomTime)
		{
			transform.position -= direction * m_zoomSpeed * Time.deltaTime;
			yield return null;
			timer += Time.deltaTime;
		}
		a_onComplete?.Invoke();

		m_zoomCR = null;
	}

	public void CreateFXFromBackgroundDisplay(LevelDisplayTemplate a_display)
	{
		if (a_display == null || a_display.FXTID == tid.NULL)
			return;

		Transform parent = null;
		switch (a_display.FXPos)
		{
			case LevelDisplayTemplate.FXPosition.AbovePlayer:
				parent = m_abovePlayerFXParent;
				break;

			case LevelDisplayTemplate.FXPosition.FrontOfPlayer:
				parent = m_frontOfPlayerFXParent;
				break;
		}

		FXObject.CreateUnderParent(a_display.FXTID, parent);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	#endregion Callbacks

}
