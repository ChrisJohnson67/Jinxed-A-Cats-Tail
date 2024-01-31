using FMODUnity;
using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UISettings
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// Settings related to the UI.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/GameSettings")]
public class GameSettings : TemplateObject
{
	//~~~~~ Definitions ~~~~~
	#region Definitions

	public const float c_SmallNumber = 0.01f;

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField, TemplateIDField(typeof(PlatformerUnitTemplate), "Player Unit Template", "")]
	private int m_playerUnitTemplateTID;

	[SerializeField, TemplateIDField(typeof(CameraController), "Camera Level Controller", "")]
	private int m_cameraLevelControllerTID;

	[SerializeField, TemplateIDField(typeof(LevelTemplate), "Test Level", "")]
	private int m_testLevelTID;

	[SerializeField, TemplateIDField(typeof(SnowballCreator), "Snowball creator", "")]
	private int m_snowballCreatorTID;

	[SerializeField]
	private float m_deathFallAcceleration = 30f;

	[Header("Jump Settings")]
	[SerializeField]
	private float m_jumpOnEnemyNoPressForce = 200f;

	[SerializeField]
	private float m_jumpOnEnemyPressForce = 500f;

	[Header("Attack Settings")]
	[SerializeField]
	private Vector2 m_swipeAttackSize;

	[Header("Input Settings")]
	[SerializeField]
	private float m_joystickUpDownThreshold = 0.6f;

	[Header("Level Settings")]
	[SerializeField]
	private float m_levelLoadIntroTime = 0.25f;

	[SerializeField]
	private float m_levelOutroTime = 1f;

	[SerializeField]
	private EventReference m_starMusic;

	[Header("Raycast Settings")]

	[SerializeField]
	private LayerMask m_groundLayers;

	[SerializeField]
	private LayerMask m_playerLayer;

	[SerializeField]
	private LayerMask m_swipeAttackLayer;

	[SerializeField]
	private LayerMask m_allCollisionLayers;

	[SerializeField]
	private LayerMask m_playerOnlyCollisionLayer;

	[SerializeField]
	private LayerMask m_enemyLayer;

	[SerializeField]
	protected float m_groundedRadius = 0.05f;

	[SerializeField]
	protected float m_topOfEnemyHeadRadius = 0.1f;

	[SerializeField]
	protected float m_topOfEnemyHeadHorzOffset = 0.05f;

	[SerializeField]
	protected float m_blockHitHorzOffset = 0.05f;

	[SerializeField]
	protected float m_warpPipeHorzOffset = 0.3f;

	[SerializeField]
	protected float m_standardRaycastSize = 0.05f;

	[Header("Player Settings")]
	[SerializeField]
	private float m_damageRecoveryTime = 1.5f;

	[SerializeField]
	private float m_damageRecoveryFreezeTime = 0.5f;

	[SerializeField]
	private int m_playerLife = 2;

	[SerializeField]
	private float m_starDuration = 7f;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int PlayerUnitTemplateTID { get { return m_playerUnitTemplateTID; } }
	public int CameraLevelControllerTID { get { return m_cameraLevelControllerTID; } }
	public int TestLevelTID { get { return m_testLevelTID; } }
	public int SnowballCreatorTID { get { return m_snowballCreatorTID; } }
	public EventReference StarMusic => m_starMusic;

	public float DeathFallAcceleration { get { return m_deathFallAcceleration; } }

	public float JumpOnEnemyNoPressForce { get { return m_jumpOnEnemyNoPressForce; } }
	public float JumpOnEnemyPressForce { get { return m_jumpOnEnemyPressForce; } }

	public float JoystickUpDownThreshold { get { return m_joystickUpDownThreshold; } }

	public float LevelLoadIntroTime { get { return m_levelLoadIntroTime; } }
	public float LevelOutroTime { get { return m_levelOutroTime; } }
	public Vector2 SwipeAttackSize { get { return m_swipeAttackSize; } }

	public LayerMask GroundLayers { get { return m_groundLayers; } }
	public LayerMask PlayerLayer { get { return m_playerLayer; } }
	public LayerMask SwipeAttackLayer { get { return m_swipeAttackLayer; } }
	public LayerMask AllCollisionLayers { get { return m_allCollisionLayers; } }
	public LayerMask PlayerOnlyCollisionLayer { get { return m_playerOnlyCollisionLayer; } }
	public LayerMask EnemyLayer { get { return m_enemyLayer; } }
	public float GroundedRadius { get { return m_groundedRadius; } }
	public float TopOfEnemyHeadRadius { get { return m_topOfEnemyHeadRadius; } }
	public float TopOfEnemyHeadHorzOffset { get { return m_topOfEnemyHeadHorzOffset; } }
	public float BlockHitHorzOffset { get { return m_blockHitHorzOffset; } }
	public float WarpPipeHorzOffset { get { return m_warpPipeHorzOffset; } }
	public float StandardRaycastSize { get { return m_standardRaycastSize; } }

	public float DamageRecoveryFreezeTime { get { return m_damageRecoveryFreezeTime; } }
	public float DamageRecoveryTime { get { return m_damageRecoveryTime; } }
	public int PlayerLife { get { return m_playerLife; } }
	public float StarDuration { get { return m_starDuration; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public int GetMaxPlayerLife()
	{
		int extraHearts = SaveManager.Instance().WorldSaveData.ExtraHeartsPurchased;
		return m_playerLife + extraHearts;
	}

	#endregion Runtime Functions
}
