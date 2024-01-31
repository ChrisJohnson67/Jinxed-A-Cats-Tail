using FMODUnity;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UISettings
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// Settings related to the UI.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/GameUISettings")]
public class GameUISettings : TemplateObject
{
	//~~~~~ Definitions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField, TemplateIDField(typeof(LoadingScreenUI), "Loading Screen", "")]
	private int m_loadingScreenTID;

	[SerializeField, TemplateIDField(typeof(TransitionOverlay), "Transition Overlay", "")]
	private int m_transitionOverlayTID;

	[SerializeField, TemplateIDField(typeof(TooltipPopupUI), "Tooltip Popup", "")]
	private int m_tooltipPopupTID;

	//[SerializeField, TemplateIDField(typeof(GenericPopupUI), "Generic Popup", "")]
	//private int m_genericPopupTID;
	//
	[SerializeField, TemplateIDField(typeof(MainMenuUI), "Title screen UI", "")]
	private int m_titleScreenTID;

	[SerializeField, TemplateIDField(typeof(OverworldUI), "Overworld UI", "")]
	private int m_overworldUITID;

	//[SerializeField, TemplateIDField(typeof(ChooseNamePopup), "Name popup UI", "")]
	//private int m_namePopupTID;
	//
	[SerializeField, TemplateIDField(typeof(CheatWindowUI), "Cheat Window UI", "")]
	private int m_cheatWindowTID;

	[SerializeField, TemplateIDField(typeof(InteractPopup), "Interact Popup", "")]
	private int m_interactPopupTID;

	[SerializeField, TemplateIDField(typeof(DialogPanel), "Dialog Player Panel", "")]
	private int m_dialogPanelPlayerTID;

	[SerializeField, TemplateIDField(typeof(DialogPanel), "Dialog Other Panel", "")]
	private int m_dialogPanelOtherTID;

	[SerializeField, TemplateIDField(typeof(LevelUI), "Level UI", "")]
	private int m_levelUITID;

	[SerializeField, TemplateIDField(typeof(GenericPopup), "Generic Popup", "")]
	private int m_genericPopupUITID;

	[SerializeField, TemplateIDField(typeof(OptionsPopup), "Options Popup", "")]
	private int m_optionsPopupUITID;

	[SerializeField, TemplateIDField(typeof(IntroUI), "Intro UI", "")]
	private int m_introUITID;

	[SerializeField, TemplateIDField(typeof(Transform), "Boss UI", "")]
	private int m_bossTitleUITID;

	[SerializeField, TemplateIDField(typeof(Credits), "Credits UI", "")]
	private int m_creditsUITID;

	[Header("Game Prefabs")]

	[SerializeField, TemplateIDField(typeof(LevelCompleteFlag), "Level Entrance Portal", "")]
	private int m_levelIntroPortalTID;

	[SerializeField, TemplateIDField(typeof(LevelCompleteFlag), "Level Complete Portal", "")]
	private int m_levelCompleteTID;

	[SerializeField]
	private float m_transitionWarpFadeTime = 0.5f;

	[SerializeField]
	private float m_transitionHoldTime = 0.5f;

	[Header("Sounds")]

	[SerializeField]
	private EventReference m_jumpSound;

	[SerializeField]
	private EventReference m_uiClickSound;

	[SerializeField]
	private EventReference m_sunflowerSound;

	[SerializeField]
	private EventReference m_levelCompleteSound;

	[SerializeField]
	private EventReference m_levelUnlockSound;

	[SerializeField]
	private EventReference m_jumpOnEnemyHeadSound;

	[SerializeField]
	private EventReference m_playerHurtSound;

	[SerializeField]
	private EventReference m_heartPickupSound;

	[SerializeField]
	private EventReference m_coinSound;

	[SerializeField]
	private EventReference m_springSound;

	[SerializeField]
	private EventReference m_mageFireballSound;

	[SerializeField]
	private EventReference m_overworldLevelEnterSound;

	[SerializeField]
	private EventReference m_portalEnterExitSound;

	[SerializeField]
	private EventReference m_shopPurchaseSound;

	[SerializeField]
	private EventReference m_overworldMoveSound;

	[SerializeField]
	private EventReference m_secretAreaFoundSound;

	[SerializeField]
	private EventReference m_bossJumpSound;

	[SerializeField]
	private EventReference m_batSwoopSound;

	[SerializeField]
	private EventReference m_bossIntroSound;

	[SerializeField]
	private EventReference m_genericEnemyDeathSound;

	[SerializeField]
	private EventReference m_pipeEntranceSound;

	[SerializeField]
	private EventReference m_snowballSound;

	[SerializeField]
	private EventReference m_scarecrowFireSound;

	[SerializeField]
	private EventReference m_boxDestroySound;

	[SerializeField]
	private EventReference m_playerLandSound;

	[SerializeField]
	private EventReference m_swipeSound;

	[SerializeField]
	private EventReference m_bossVictorySound;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int LoadingScreenTID { get { return m_loadingScreenTID; } }
	public int TransitionOverlayTID { get { return m_transitionOverlayTID; } }
	public int TooltipPopup { get { return m_tooltipPopupTID; } }
	//public int GenericPopupUITID { get { return m_genericPopupTID; } }
	public int TitleScreenTID { get { return m_titleScreenTID; } }
	//public int NamePopupTID { get { return m_namePopupTID; } }
	public int CheatWindowTID { get { return m_cheatWindowTID; } }
	public int InteractPopupTID { get { return m_interactPopupTID; } }
	public int DialogPanelPlayerTID { get { return m_dialogPanelPlayerTID; } }
	public int DialogPanelOtherTID { get => m_dialogPanelOtherTID; }
	public int LevelUITID => m_levelUITID;
	public int GenericPopupUITID => m_genericPopupUITID;
	public int OverworldUITID => m_overworldUITID;
	public int OptionsPopupUITID => m_optionsPopupUITID;
	public int IntroUITID => m_introUITID;
	public int BossTitleUITID => m_bossTitleUITID;
	public int CreditsUITID => m_creditsUITID;


	public int LevelIntroPortalTID => m_levelIntroPortalTID;
	public int LevelCompleteTID => m_levelCompleteTID;

	public float TransitionWarpFadeTime { get { return m_transitionWarpFadeTime; } }
	public float TransitionHoldTime { get { return m_transitionHoldTime; } }

	public EventReference JumpSound { get => m_jumpSound; set => m_jumpSound = value; }
	public EventReference UiClickSound { get => m_uiClickSound; set => m_uiClickSound = value; }
	public EventReference SunflowerSound { get => m_sunflowerSound; set => m_sunflowerSound = value; }
	public EventReference LevelCompleteSound { get => m_levelCompleteSound; set => m_levelCompleteSound = value; }
	public EventReference LevelUnlockSound { get => m_levelUnlockSound; set => m_levelUnlockSound = value; }
	public EventReference JumpOnEnemyHeadSound { get => m_jumpOnEnemyHeadSound; set => m_jumpOnEnemyHeadSound = value; }
	public EventReference PlayerHurtSound { get => m_playerHurtSound; set => m_playerHurtSound = value; }
	public EventReference HeartPickupSound { get => m_heartPickupSound; set => m_heartPickupSound = value; }
	public EventReference CoinSound { get => m_coinSound; set => m_coinSound = value; }
	public EventReference SpringSound { get => m_springSound; set => m_springSound = value; }
	public EventReference MageFireballSound { get => m_mageFireballSound; set => m_mageFireballSound = value; }
	public EventReference OverworldLevelEnterSound { get => m_overworldLevelEnterSound; set => m_overworldLevelEnterSound = value; }
	public EventReference PortalEnterExitSound { get => m_portalEnterExitSound; set => m_portalEnterExitSound = value; }
	public EventReference ShopPurchaseSound { get => m_shopPurchaseSound; set => m_shopPurchaseSound = value; }
	public EventReference OverworldMoveSound { get => m_overworldMoveSound; set => m_overworldMoveSound = value; }
	public EventReference SecretAreaFoundSound { get => m_secretAreaFoundSound; set => m_secretAreaFoundSound = value; }
	public EventReference BossJumpSound { get => m_bossJumpSound; set => m_bossJumpSound = value; }
	public EventReference BatSwoopSound { get => m_batSwoopSound; set => m_batSwoopSound = value; }
	public EventReference BossIntroSound { get => m_bossIntroSound; set => m_bossIntroSound = value; }
	public EventReference GenericEnemyDeathSound { get => m_genericEnemyDeathSound; set => m_genericEnemyDeathSound = value; }
	public EventReference PipeEntranceSound { get => m_pipeEntranceSound; set => m_pipeEntranceSound = value; }
	public EventReference SnowballSound { get => m_snowballSound; set => m_snowballSound = value; }
	public EventReference ScarecrowFireSound { get => m_scarecrowFireSound; set => m_scarecrowFireSound = value; }
	public EventReference BoxDestroySound { get => m_boxDestroySound; set => m_boxDestroySound = value; }
	public EventReference PlayerLandSound { get => m_playerLandSound; set => m_playerLandSound = value; }
	public EventReference SwipeSound { get => m_swipeSound; set => m_swipeSound = value; }
	public EventReference BossVictorySound { get => m_bossVictorySound; set => m_bossVictorySound = value; }


	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	#endregion Runtime Functions
}
