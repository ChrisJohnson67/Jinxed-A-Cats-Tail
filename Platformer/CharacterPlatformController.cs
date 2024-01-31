using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CharacterPlatformController
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[RequireComponent(typeof(RegisterInputHelper))]
public class CharacterPlatformController : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions
	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private PlayerUnitModel m_unit;

	[SerializeField]
	private Transform m_groundCollisionTransform;

	[SerializeField]
	protected float m_moveSpeed = 40f;

	[SerializeField]
	protected float m_movementSmoothing = .05f;

	[SerializeField]
	private bool m_airControl = true;

	[Header("Jump Settings")]

	[SerializeField]
	private int m_jumpsAllowed = 1;

	[SerializeField]
	protected float m_jumpForce = 40f;

	[SerializeField]
	protected float m_releaseJumpVelocityMod = 0.5f;

	[SerializeField]
	protected float m_coyoteJumpTime;

	[SerializeField]
	protected float m_bufferedJumpTime;

	[Header("Attack Settings")]

	[SerializeField]
	private float m_attackCooldown = 1f;

	private bool m_requestedJump = false;
	private bool m_grounded = true;
	private float m_horizonalMove = 0f;
	private int m_jumpsRemaining;
	private bool m_isJumpPressed;
	private bool m_canReleaseJumpToFall;

	private bool m_requestedAttack;
	private float m_attackCooldownTimer = 0f;

	private Coroutine m_coyoteCR;
	private bool m_canCoyoteJump;

	private Coroutine m_bufferJumpCR;
	private bool m_hasBufferedJump;


	public static Action<bool> OnLand; //did the player land on an enemy
	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public bool IsHoldingJump { get { return m_isJumpPressed; } }
	public bool IsGrounded { get { return m_grounded; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public void Awake()
	{
		m_jumpsRemaining = m_jumpsAllowed;

		Spring.OnSpring += OnUsedSpring;
	}

	private void OnDestroy()
	{
		Spring.OnSpring -= OnUsedSpring;
	}

	private void Update()
	{
		if (!LevelManager.HasInstance || LevelManager.Instance.Paused || m_unit == null || m_unit.UnitInstance == null || !m_unit.UnitInstance.IsAlive)
			return;

		m_isJumpPressed = false;
		m_horizonalMove = 0f;

		if (m_attackCooldownTimer > 0f)
		{
			m_attackCooldownTimer -= Time.deltaTime;
		}

		if (m_unit.PlayerInstance.CanUseInput())
		{
			CheckForJumpInput();
			CheckForMovementInput();
			CheckForAttackInput();
		}

		m_horizonalMove = m_horizonalMove * m_moveSpeed;
	}

	private void CheckForJumpInput()
	{
		if (InputListenerManager.Instance.WasGamepadButtonPressedThisFrame(GamepadButton.A, gameObject) ||
			InputListenerManager.Instance.WasKeyPressedThisFrame(Key.Space, gameObject))
		{
			m_requestedJump = true;
			if (!m_grounded)
			{
				m_hasBufferedJump = true;
				if (m_bufferJumpCR != null)
				{
					StopCoroutine(m_bufferJumpCR);
				}
				m_bufferJumpCR = StartCoroutine(BufferJumpTimeCR());
			}
		}

		if (InputListenerManager.Instance.IsGamepadButtonHeldDown(GamepadButton.A, gameObject) ||
			InputListenerManager.Instance.IsKeyHeldDown(Key.Space, gameObject))
		{
			m_isJumpPressed = true;
		}

		if (InputListenerManager.Instance.WasGamepadButtonReleasedThisFrame(GamepadButton.A, gameObject) ||
			InputListenerManager.Instance.WasKeyReleasedThisFrame(Key.Space, gameObject))
		{
			if (m_hasBufferedJump)
			{
				m_hasBufferedJump = false;
			}

			if (m_unit.Rigidbody.velocity.y > 0f && m_canReleaseJumpToFall)
			{
				m_unit.SetVelocity(new Vector2(m_unit.Rigidbody.velocity.x, m_unit.Rigidbody.velocity.y * m_releaseJumpVelocityMod));
				m_canReleaseJumpToFall = false;
			}
		}
	}

	private void CheckForMovementInput()
	{
		var gamepadDirection = InputListenerManager.Instance.GetLeftJoystickDirection(gameObject, true);
		if (InputListenerManager.Instance.IsKeyHeldDown(Key.RightArrow, gameObject) || gamepadDirection.x > 0f)
		{
			m_horizonalMove = 1f;
		}
		else if (InputListenerManager.Instance.IsKeyHeldDown(Key.LeftArrow, gameObject) || gamepadDirection.x < 0f)
		{
			m_horizonalMove = -1f;
		}
	}

	private void CheckForAttackInput()
	{
		if (InputListenerManager.Instance.WasKeyPressedThisFrame(Key.LeftCtrl, gameObject) ||
			InputListenerManager.Instance.WasGamepadButtonPressedThisFrame(GamepadButton.X, gameObject))
		{
			m_requestedAttack = true;
		}
	}

	private void FixedUpdate()
	{
		if (!LevelManager.HasInstance || LevelManager.Instance.Paused || m_unit == null || m_unit.UnitInstance == null || !m_unit.UnitInstance.IsAlive)
			return;

		//check if grounded
		CheckGrounded();

		if (CanJump())
		{
			ExecuteJump();
		}

		if (m_grounded || m_airControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(m_horizonalMove * Time.fixedDeltaTime, m_unit.Rigidbody.velocity.y);
			// And then smoothing it out and applying it to the character
			if (m_unit.PlayerInstance.CanMoveWithInput)
				m_unit.SetVelocity(targetVelocity);

			if (Mathf.Abs(targetVelocity.x) > GameSettings.c_SmallNumber)
			{
				m_unit.SetFacingRight(targetVelocity.x >= 0f);
			}
		}

		//make sure the unit doesn't go past the left boundary
		var leftBoundary = LevelManager.Instance.CurrentLevel.LeftCameraBound + m_unit.Collider.bounds.size.x * 0.5f;
		var rightBoundary = LevelManager.Instance.CurrentLevel.RightCameraBound + m_unit.Collider.bounds.size.x * 0.5f;
		if (m_unit.Rigidbody.position.x < leftBoundary)
		{
			m_unit.Rigidbody.position = new Vector2(leftBoundary, m_unit.Rigidbody.position.y);
		}
		else if (m_unit.Rigidbody.position.x > rightBoundary)
		{
			m_unit.Rigidbody.position = new Vector2(rightBoundary, m_unit.Rigidbody.position.y);
		}

		if (CanAttack())
		{
			ExecuteAttack();
		}

		m_requestedJump = false;
		m_requestedAttack = false;
	}

	private bool CanJump()
	{
		return (m_grounded || m_canCoyoteJump || m_unit.FallingOnPlatform) && (m_requestedJump || m_hasBufferedJump) && m_jumpsRemaining > 0;
	}

	private void ExecuteJump()
	{
		m_jumpsRemaining--;
		m_grounded = false;
		m_canReleaseJumpToFall = true;
		m_unit.SetVelocity(new Vector2(m_unit.Rigidbody.velocity.x, 0f));
		m_unit.AddForce(new Vector2(0f, m_jumpForce));

		m_unit.SetJumping(true);
		DisableCoyoteJump();

		SoundManager.Instance.PlaySound(GameManager.Instance.UISettings.JumpSound);

		m_hasBufferedJump = false;
	}

	private bool CanAttack()
	{
		return m_requestedAttack && m_attackCooldownTimer <= 0f;
	}

	private void ExecuteAttack()
	{
		m_attackCooldownTimer = m_attackCooldown;

		var halfSwipeSize = GameManager.Instance.GameSettings.SwipeAttackSize * 0.5f;
		int hits = Physics2D.OverlapAreaNonAlloc(new Vector2(m_unit.SwipeAttackNode.position.x - halfSwipeSize.x, m_unit.SwipeAttackNode.position.y - halfSwipeSize.y),
													new Vector2(m_unit.SwipeAttackNode.position.x + halfSwipeSize.x, m_unit.SwipeAttackNode.position.y + halfSwipeSize.y),
														GameManager.sm_colliderHits, GameManager.Instance.GameSettings.SwipeAttackLayer);
		for (int i = 0; i < hits; i++)
		{
			var hitInfo = GameManager.sm_colliderHits[i];
			if (hitInfo.gameObject != null && hitInfo.gameObject)
			{
				var swipeTarget = hitInfo.gameObject.GetComponent<ISwipeTarget>();
				if (swipeTarget != null)
				{
					swipeTarget.HitBySwipe();
				}
			}
		}
		m_unit.CreateAttackFX();
	}

	private void CheckGrounded()
	{
		bool wasGrounded = m_grounded;
		m_grounded = false;


		if (Mathf.Abs(m_unit.Rigidbody.velocity.y) <= GameSettings.c_SmallNumber)
		{
			int hits = Physics2D.OverlapAreaNonAlloc(new Vector2(m_unit.Collider.bounds.min.x, m_unit.Collider.bounds.min.y),
													new Vector2(m_unit.Collider.bounds.max.x, m_unit.Collider.bounds.min.y + GameManager.Instance.GameSettings.GroundedRadius),
														GameManager.sm_colliderHits, GameManager.Instance.GameSettings.GroundLayers);
			for (int i = 0; i < hits; i++)
			{
				if (GameManager.sm_colliderHits[i].gameObject != gameObject)
				{
					m_grounded = true;
					m_jumpsRemaining = m_jumpsAllowed;
					m_canReleaseJumpToFall = false;
					if (!wasGrounded)
					{
						OnLand?.Invoke(false);
						DisableCoyoteJump();
						m_unit.SetVelocity(new Vector2(m_unit.Rigidbody.velocity.x, 0f));

						m_unit.SetJumping(false);
					}
					break;
				}
			}
		}

		m_unit.ShowRunningFX(m_grounded && Mathf.Abs(m_unit.Rigidbody.velocity.x) > GameSettings.c_SmallNumber);

		//start a coyote jump timer
		if (wasGrounded && !m_grounded)
		{
			DisableCoyoteJump();
			m_coyoteCR = StartCoroutine(CoyoteTimeCR());
		}
	}

	private void DisableCoyoteJump()
	{
		if (m_coyoteCR != null)
		{
			StopCoroutine(m_coyoteCR);
		}
		m_canCoyoteJump = false;
	}

	private IEnumerator CoyoteTimeCR()
	{
		m_canCoyoteJump = true;
		float timer = 0;
		while (timer < m_coyoteJumpTime)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		m_canCoyoteJump = false;
		m_coyoteCR = null;
	}

	private IEnumerator BufferJumpTimeCR()
	{
		float timer = 0;
		while (timer < m_bufferedJumpTime)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		m_hasBufferedJump = false;
		m_bufferJumpCR = null;
	}

	private void OnUsedSpring()
	{
		m_canReleaseJumpToFall = false;
	}

	#endregion Runtime Functions

#if UNITY_EDITOR
#endif
}