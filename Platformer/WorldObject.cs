using System;
using System.Collections;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// WorldObject
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class WorldObject : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	protected Transform m_parentTransformNode;

	[SerializeField]
	protected Transform m_groundedNode;

	[SerializeField]
	protected Collider2D m_collider;

	[SerializeField]
	protected Rigidbody2D m_rigidBody;

	//--- NonSerialized ---
	private Coroutine m_waitForGroundedCR;
	protected Vector2 m_pausedVelocity;
	protected RigidbodyType2D m_pausedBodyType;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public Transform ParentTransformNode { get { return m_parentTransformNode; } }
	public Collider2D Collider { get { return m_collider; } }
	public Rigidbody2D Rigidbody { get { return m_rigidBody; } }
	public Transform GroundedNode { get { return m_groundedNode; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	protected virtual void Awake()
	{
		LevelManager.OnPaused += OnPause;
	}

	protected virtual void OnDestroy()
	{
		LevelManager.OnPaused -= OnPause;
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public virtual void Cleanup()
	{
		StopAllCoroutines();
		GameManager.Instance.DeleteObject(gameObject);
	}

	public virtual void SetVelocity(Vector2 a_velocity)
	{
		if (m_rigidBody != null)
			m_rigidBody.velocity = a_velocity;
	}

	public void AddForce(Vector2 a_force)
	{
		if (m_rigidBody != null)
			m_rigidBody.AddForce(a_force);
	}

	public void SetKinematic()
	{
		if (m_rigidBody != null)
			m_rigidBody.bodyType = RigidbodyType2D.Kinematic;
	}

	public void SetSimulated(bool a_sim)
	{
		if (m_rigidBody != null)
			m_rigidBody.simulated = a_sim;
	}

	public void SetDynamic()
	{
		if (m_rigidBody != null)
			m_rigidBody.bodyType = RigidbodyType2D.Dynamic;
	}

	public void SetInterpolateRB(bool a_interpolate)
	{
		if (m_rigidBody != null)
			m_rigidBody.interpolation = a_interpolate ? RigidbodyInterpolation2D.Interpolate : RigidbodyInterpolation2D.None;
	}

	public void EnableCollider(bool a_enable)
	{
		if (m_collider != null)
			m_collider.enabled = a_enable;
	}

	public virtual void SetPaused(bool a_paused)
	{
		if (a_paused)
		{
			if (m_rigidBody != null)
			{
				m_pausedVelocity = m_rigidBody.velocity;
				m_pausedBodyType = m_rigidBody.bodyType;
				SetKinematic();
			}
			SetVelocity(Vector2.zero);
		}
		else
		{
			if (m_rigidBody != null)
			{
				m_rigidBody.bodyType = m_pausedBodyType;
			}
			SetVelocity(m_pausedVelocity);
		}
	}

	public void StopGroundedCoroutine()
	{
		if (m_waitForGroundedCR != null)
		{
			StopCoroutine(m_waitForGroundedCR);
			m_waitForGroundedCR = null;
		}
	}

	public void WaitForGrounded(Action<WorldObject, GameObject> a_onGrounded)
	{
		StopGroundedCoroutine();
		m_waitForGroundedCR = StartCoroutine(WaitForGroundedCR(a_onGrounded));
	}

	private IEnumerator WaitForGroundedCR(Action<WorldObject, GameObject> a_onGrounded)
	{
		var groundedObject = GetGroundedObject();
		while (groundedObject == null)
		{
			yield return null;
			groundedObject = GetGroundedObject();
		}

		a_onGrounded?.Invoke(this, groundedObject);
		m_waitForGroundedCR = null;
	}

	public GameObject GetGroundedObject()
	{
		if (Mathf.Abs(m_rigidBody.velocity.y) <= GameSettings.c_SmallNumber)
		{
			int hits = Physics2D.OverlapAreaNonAlloc(new Vector2(m_collider.bounds.min.x, m_collider.bounds.min.y),
													new Vector2(m_collider.bounds.max.x, m_collider.bounds.min.y + GameManager.Instance.GameSettings.GroundedRadius),
														GameManager.sm_colliderHits, GameManager.Instance.GameSettings.GroundLayers);
			for (int i = 0; i < hits; i++)
			{
				var hitInfo = GameManager.sm_colliderHits[i];
				if (hitInfo.gameObject != gameObject)
				{
					return hitInfo.gameObject;
				}
			}
		}
		return null;
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	protected void OnPause(bool a_paused)
	{
		SetPaused(a_paused);
	}

	#endregion Callbacks

#if UNITY_EDITOR
	protected virtual void OnValidate()
	{
		if (m_parentTransformNode == null)
		{
			var obj = gameObject.FindObjectWithName("parentNode");
			if (obj != null)
			{
				m_parentTransformNode = obj.transform;
			}
		}

		if (m_groundedNode == null)
		{
			var obj = gameObject.FindObjectWithName("groundNode");
			if (obj != null)
			{
				m_groundedNode = obj.transform;
			}
		}

		if (m_rigidBody == null)
			m_rigidBody = GetComponentInChildren<Rigidbody2D>();

		if (m_rigidBody != null)
		{
			m_rigidBody.freezeRotation = true;
		}

		if (m_collider == null)
		{
			var collider = GetComponentInChildren<Collider2D>();
			if (collider != null && !collider.isTrigger)
			{
				m_collider = collider;
			}
		}
	}
#endif
}