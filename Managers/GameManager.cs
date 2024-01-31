using Platform.UIManagement;
using Platform.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoSingleton<GameManager>
{
	public const string c_EnemyTriggerTag = "EnemyTrigger";
	public const string c_InteractionTag = "Interaction";
	public const string c_EnemyTag = "Enemy";
	public const string c_PlayerTag = "Player";
	public const string c_WarpPipeTag = "WarpPipe";
	public const string c_LevelCompleteFlagTag = "CompleteFlag";

	public const string c_PlayerLayer = "Player";
	public const string c_EnemyLayer = "Enemy";


	[SerializeField]
	private UICanvas m_mainCanvas = null;

	[SerializeField]
	private Camera m_uiCamera;

	[SerializeField]
	private RectTransform m_canvasParent;

	[SerializeField]
	private Transform m_fxParent;

	[SerializeField]
	private Transform m_unitParent;

	[SerializeField]
	private GameUISettings m_uiSettings = null;

	[SerializeField]
	private CurrencySettings m_currencySettings = null;

	[SerializeField]
	private GameSettings m_gameSettings = null;

	[SerializeField]
	private InputSettings m_inputSettings = null;

	[SerializeField]
	private LevelSettings m_levelSettings;

	public static Collider2D[] sm_colliderHits = new Collider2D[8];

	public UICanvas UICanvas { get { return m_mainCanvas; } }
	public Canvas MainCanvas { get { return m_mainCanvas.Canvas; } }
	public RectTransform CanvasParent { get { return m_canvasParent; } }
	public Transform FXParent { get { return m_fxParent; } }
	public Transform UnitParent { get { return m_unitParent; } }
	public GameUISettings UISettings { get { return m_uiSettings; } }
	public CurrencySettings CurrencySettings { get { return m_currencySettings; } }
	public GameSettings GameSettings { get { return m_gameSettings; } }
	public InputSettings InputSettings { get { return m_inputSettings; } }
	public LevelSettings LevelSettings => m_levelSettings;


	protected override void Awake()
	{
		base.Awake();
		SaveManager.Instance().Load();
		if (SaveManager.Instance().UserSaveData.Resolution != Vector2Int.zero)
		{
			var res = SaveManager.Instance().UserSaveData.Resolution;
			Screen.SetResolution(res.x, res.y, SaveManager.Instance().UserSaveData.Fullscreen);
		}
	}

	private IEnumerator Start()
	{
		Application.targetFrameRate = 60;

		DialogManager.Instance.VerifyCreate();

		LoadingScreenUI.Show();
		yield return null;

		Shader.WarmupAllShaders();

		UIManager.Instance.OpenUI<MainMenuUI>(m_uiSettings.TitleScreenTID);
		yield return null;
		LoadingScreenUI.Hide();
	}

	private void Update()
	{
		if (Keyboard.current[Key.Backquote].wasPressedThisFrame)
		{
			CheatWindowUI.Open();
		}

	}

	public void SetDepthCameraActive(bool a_active)
	{
		UIUtils.SetActive(m_uiCamera, a_active);
	}

	public void DeleteObject(GameObject a_object, float a_time)
	{
		if (a_object != null)
		{
			GameObject.Destroy(a_object, a_time);
		}
	}

	public void DeleteObject(GameObject a_object)
	{
		if (a_object != null)
		{
			GameObject.Destroy(a_object);
		}
	}

	public void DeleteObject(Component a_comp)
	{
		if (a_comp != null)
		{
			DeleteObject(a_comp.gameObject);
		}
	}

	public void UnloadResources()
	{
		GameObjectPool.UnloadEmptyPools();
		Resources.UnloadUnusedAssets();
	}

	protected override void OnApplicationQuit()
	{
		base.OnApplicationQuit();
		GameObjectPool.ClearAll();
	}
}
